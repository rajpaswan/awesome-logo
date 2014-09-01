using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;

namespace AwesomeLogoPhone
{
    public partial class SamplePage : PhoneApplicationPage
    {
        public SamplePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync("Samples");
            sample_stack.Children.Clear();
            if (folder != null)
            {
                var files = await folder.GetFilesAsync();
                for (int i = 0; i < files.Count; i++)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = files[i].Name.Split('.')[0];
                    tb.Tap += open_sample;
                    tb.FontFamily = new System.Windows.Media.FontFamily("consolas");
                    tb.Margin = new Thickness(50, 10, 0, 10);
                    sample_stack.Children.Add(tb);
                }
            }
            else
            {
                MessageBox.Show("folder not found!");
            }
            base.OnNavigatedTo(e);
        }

        private void open_sample(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string filename = (sender as TextBlock).Text;
            ScriptPage.foldername = "Samples";
            ScriptPage.filename = filename;
            this.NavigationService.Navigate(new Uri("/ScriptPage.xaml", UriKind.Relative));
        }
    }
}