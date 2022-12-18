using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Selenium_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            String[] scrapeOptions = { "Youtube", "ICTJobs", "Wikipedia" };
            scrapeOptionsDropdown.ItemsSource = scrapeOptions;
        }

        private void scrapeOptionsSubmit_Click(object sender, RoutedEventArgs e)
        {
            int scrapeOption = scrapeOptionsDropdown.SelectedIndex;
            switch (scrapeOption)
            {
                case 0:
                    Display.Content = new Youtube();
                    break;
                case 1:
                    Display.Content = new ICTJobs();
                    break;
                case 2:
                    Display.Content = new Wikipedia();
                    break;
                default:
                    break;
            }
        }
    }
}
