using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Selenium_WPF
{
    /// <summary>
    /// Interaction logic for Youtube.xaml
    /// </summary>
    public partial class Youtube : Page
    {
        public Youtube()
        {
            InitializeComponent();

            String[] sortingOptions = { "Relevance", "Upload date", "View count", "Rating"};
            sortingOptionsDropdown.ItemsSource = sortingOptions;
        }

        private void scrapeStart_Click(object sender, RoutedEventArgs e)
        {
            String query = queryTextbox.Text;
            int amount = Int32.Parse(amountTextbox.Text);
            int sortingIndex = sortingOptionsDropdown.SelectedIndex;

            List<Dictionary<String, String>> dataYoutube = Scrapers.ScrapeYoutube(query, sortingIndex, amount);
            Utils.writeToJson(dataYoutube, "youtube-data.json");
            statusText.Content = "Data has been saved in \"/files/\".";

        }

        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
