using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Storage;

namespace AwesomeLogoPhone
{
    public partial class FilePage : PhoneApplicationPage
    {
        public FilePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync("Scripts");
            file_stack.Children.Clear();
            if(folder!=null)
            {
                var files = await folder.GetFilesAsync();
                for(int i=0;i<files.Count;i++)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = files[i].Name.Split('.')[0];
                    tb.Tap += open_file;
                    tb.FontFamily = new System.Windows.Media.FontFamily("consolas");
                    tb.Margin = new Thickness(50, 10, 0, 10);
                    file_stack.Children.Add(tb);
                }
            }
            else
            {
                MessageBox.Show("Error: folder not found!"); 
            }
            base.OnNavigatedTo(e);
        }

        private void open_file(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string filename=(sender as TextBlock).Text;
            ScriptPage.foldername = "Scripts";
            ScriptPage.filename = filename;
            this.NavigationService.Navigate(new Uri("/ScriptPage.xaml", UriKind.Relative));
        }
    }
}