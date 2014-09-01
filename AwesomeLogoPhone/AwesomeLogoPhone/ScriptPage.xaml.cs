using Microsoft.Phone.Controls;
using System;
using System.IO;
using System.Windows;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AwesomeLogoPhone
{
    public partial class ScriptPage : PhoneApplicationPage
    {
        public static string filename;
        public static string foldername;
        public static string filecontent;

        public ScriptPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (filename != null && filename!="")
            {
                filename_block.Text = filename;
                var resource = Application.GetResourceStream(new Uri(foldername+"/"+filename+".txt", UriKind.Relative));

                StreamReader streamReader = new StreamReader(resource.Stream);
                string data = streamReader.ReadToEnd();
                input_box.Text = data;
                filecontent = data;
            }
            else
            {
                filename_block.Text = "script";
                input_box.Text = "";
            }
            base.OnNavigatedTo(e);
        }
        private void run_button_Click(object sender, EventArgs e)
        {
            filecontent = input_box.Text.Trim();
            TurtlePage.script = filecontent;
            this.NavigationService.Navigate(new Uri("/TurtlePage.xaml", UriKind.Relative));
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            var flyout = new SaveControl();
            LayoutRoot.Children.Add(flyout);
        }

        private void open_button_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FilePage.xaml", UriKind.Relative));
        }

        private void close_button_Click(object sender, EventArgs e)
        {
            foldername = null;
            filename = null;
            input_box.Text = "";
            TurtlePage.script = "";
        }

        private void input_box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            filecontent = input_box.Text;
        }

        //class end
    }
}