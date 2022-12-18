using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace Selenium_WPF
{
    internal class Scrapers
    {
        public static List<Dictionary<String, String>> ScrapeYoutube(String query, int sortingIndex, int amount)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("start-maximized");
            //chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("log-level=3");

            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl("https://www.youtube.com/");

            String XPathCookies = "//*[@id=\"content\"]/div[2]/div[6]/div[1]/ytd-button-renderer[1]/yt-button-shape/button";
            IWebElement cookiesButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathCookies)));
            cookiesButton.Click();

            String XPathSearchBar = "//input[@id=\"search\"]";
            IWebElement searchBar = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathSearchBar)));
            searchBar.Click();
            searchBar.SendKeys(query);
            searchBar.Submit();

            // Open filters
            String XPathFilterMenuButton = "//*[@id=\"container\"]/ytd-toggle-button-renderer";
            IWebElement filterMenuButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathFilterMenuButton)));
            filterMenuButton.Click();

            // Sort
            String XPathSortingOption = $"//*[@id=\"collapse-content\"]/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[{sortingIndex + 1}]/a";
            IWebElement sortingButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathSortingOption)));
            sortingButton.Click();

            Thread.Sleep(2500);

            ReadOnlyCollection<IWebElement> videos = driver.FindElements(By.XPath("//div[@id=\"contents\"]/ytd-video-renderer"));
            List<Dictionary<String, String>> data = new List<Dictionary<String, String>>();

            for (int i = 0; i < amount; i++)
            {
                IWebElement video = videos.ElementAt(i);

                String id = $"{i + 1}";
                String title = video.FindElement(By.XPath(".//*[@id=\"video-title\"]")).Text;
                String channel = video.FindElement(By.XPath(".//*[@id=\"channel-info\"]/*[@id=\"channel-name\"]")).Text;
                String views = video.FindElement(By.XPath(".//*[@id=\"metadata-line\"]/span[1]")).Text;
                String uploadDate = video.FindElement(By.XPath(".//*[@id=\"metadata-line\"]/span[2]")).Text;
                String url = video.FindElement(By.XPath(".//a[@id=\"video-title\"]")).GetAttribute("href");

                Dictionary<String, String> videoData = new Dictionary<String, String>();
                videoData.Add("id", id);
                videoData.Add("title", title);
                videoData.Add("channel", channel);
                videoData.Add("views", views);
                videoData.Add("uploadDate", uploadDate);
                videoData.Add("url", url);
                data.Add(videoData);
            }

            return data;
        }

        public static List<Dictionary<String, String>> ScrapeICTJobs(String query, int sortingIndex, int amount)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("start-maximized");
            //chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("log-level=3");

            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl("https://www.ictjob.be/");

            String XPathSearchBar = "//input[@id=\"keywords-input\"]";
            IWebElement searchBar = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathSearchBar)));
            searchBar.Click();
            searchBar.SendKeys(query);
            searchBar.Submit();

            String XPathSorting = $"//*[@id=\"search-result\"]/div[1]/div[2]/div/div[2]/span[{sortingIndex + 1}]";
            IWebElement sortingButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathSorting)));
            js.ExecuteScript("arguments[0].scrollIntoView();", sortingButton);
            sortingButton.Click();

            while (driver.FindElement(By.Id("search-result-body")).GetCssValue("opacity") == "0.5")
            {
                Console.WriteLine("Waiting for sorting...");
                Thread.Sleep(250);
            }

            IWebElement searchResults = driver.FindElement(By.Id("search-result-body"));
            ReadOnlyCollection<IWebElement> jobs = searchResults.FindElements(By.XPath(".//div/ul/li/span[@class=\"job-info\"]"));

            List<Dictionary<String, String>> data = new List<Dictionary<String, String>>();


            for (int i = 0; i < amount; i++)
            {
                IWebElement job = jobs.ElementAt(i);
                String id = $"{i + 1}";
                String title = job.FindElement(By.XPath(".//h2[@class=\"job-title\"]")).Text;
                String company = job.FindElement(By.ClassName("job-company")).Text;
                String location = job.FindElement(By.XPath(".//*[@class=\"job-location\"]/span/span")).Text;
                String keywords = job.FindElement(By.ClassName("job-keywords")).Text;
                String url = job.FindElement(By.TagName("a")).GetAttribute("href");

                Dictionary<String, String> jobData = new Dictionary<String, String>();
                jobData.Add("id", id);
                jobData.Add("title", title);
                jobData.Add("company", company);
                jobData.Add("location", location);
                jobData.Add("keywords", keywords);
                jobData.Add("url", url);
                data.Add(jobData);
            }

            return data;
        }

        public static Dictionary<String, String> ScrapeWikipedia(String query, int languageIndex)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("start-maximized");
            //chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("log-level=3");

            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl("https://www.wikipedia.org/");

            SelectElement languageSelect = new SelectElement(driver.FindElement(By.XPath($"//*[@id=\"searchLanguage\"]")));
            languageSelect.SelectByIndex(languageIndex);

            IWebElement searchBar = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("searchInput")));
            searchBar.Click();
            searchBar.SendKeys(query);
            searchBar.Submit();

            IWebElement infobox = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("infobox")));
            //IWebElement infoboxTitle = infobox.FindElement(By.ClassName("infobox-above"));

            Dictionary<String, String> data = new Dictionary<String, String>();
            data.Add("Query", query);
            ReadOnlyCollection<IWebElement> infoboxRows = infobox.FindElements(By.TagName("tr"));
            String lastHeader = "";
            foreach (IWebElement row in infoboxRows)
            {
                Dictionary<String, String> video = new Dictionary<String, String>();
                String labelText = "";
                String dataText = "";

                ReadOnlyCollection<IWebElement> headers = row.FindElements(By.ClassName("infobox-header"));
                IWebElement header;

                if (headers.Count > 0)
                {
                    header = headers.First();
                    lastHeader = header.Text;
                }

                try
                {
                    IWebElement label = row.FindElement(By.TagName("th"));
                    labelText = label.Text;
                    if (labelText.StartsWith("•"))
                    {
                        int lastLocation = labelText.IndexOf("•");
                        labelText = lastHeader + " -" + labelText.Substring(1);
                    }
                    else
                    {
                        lastHeader = labelText;
                    }

                }
                catch { }
                try
                {
                    IWebElement infoboxData = row.FindElement(By.TagName("td"));
                    dataText = infoboxData.Text;
                }
                catch { }

                if (labelText != "" && dataText != "")
                {
                    data.Add(labelText, dataText);
                }


            }

            return data;

        }
    }
}
