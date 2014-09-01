using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace AwesomeLogoPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        //Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            //ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            //ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            //appBarButton.Text = AppResources.AppBarButtonText;
            //ApplicationBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void learn_button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ScriptPage.xaml", UriKind.Relative));
        }

        private void sample_button_Click(object sender, RoutedEventArgs e)
        {
            TurtlePage.script = "";
            ScriptPage.filename = "";
            this.NavigationService.Navigate(new Uri("/SamplePage.xaml", UriKind.Relative));
        }

        private void file_button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FilePage.xaml", UriKind.Relative));
        }

        private void puzzle_button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/PuzzlePage.xaml", UriKind.Relative));
        }
    }
}