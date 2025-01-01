using System.Windows;
using System.Windows.Controls;
using SportsEventTracker.Models;
using SportsEventTracker.WPF.Pages;

namespace SportsEventTracker.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigateToTeams(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TeamsPage());
        }

        private void NavigateToMatches(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MatchPage());
        }
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Exit the application
        }
    }
}
