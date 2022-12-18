using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Selenium_WPF
{
    /// <summary>
    /// Interaction logic for Wikipedia.xaml
    /// </summary>
    public partial class Wikipedia : Page
    {
        public Wikipedia()
        {
            InitializeComponent();

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("start-maximized");
            chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("log-level=3");

            IWebDriver driver = new ChromeDriver(service, chromeOptions);

            driver.Navigate().GoToUrl("https://www.wikipedia.org/");

            List<String> languages = new List<String>();

            ReadOnlyCollection<IWebElement> languageElements = driver.FindElements(By.XPath("//*[@id=\"searchLanguage\"]/option"));
            foreach (IWebElement languageElement in languageElements)
            {
                languages.Add(languageElement.Text);
            }
            languageOptionsDropdown.ItemsSource = languages;
        }

        private void scrapeStart_Click(object sender, RoutedEventArgs e)
        {
            String query = queryTextbox.Text;
            int languageIndex = languageOptionsDropdown.SelectedIndex;

            Dictionary<String, String> dataWikipedia = Scrapers.ScrapeWikipedia(query, languageIndex);
            Utils.writeToJson(dataWikipedia, "wikipedia-data.json");
            statusText.Content = "Data has been saved in \"/files/\".";
        }
    }
}
