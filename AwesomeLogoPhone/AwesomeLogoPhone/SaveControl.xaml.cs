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
using Windows.Storage.Streams;

namespace AwesomeLogoPhone
{
    public partial class SaveControl : UserControl
    {
        public SaveControl()
        {
            InitializeComponent();
            this.Loaded += SaveControl_Loaded;
        }

        void SaveControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        private async void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (savename_box.Text.Trim() != "" && ScriptPage.filecontent!=null && ScriptPage.filecontent.Trim()!="")
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync("Scripts");
                StorageFile newFile = await folder.CreateFileAsync(savename_box.Text + ".txt", CreationCollisionOption.GenerateUniqueName);
                using (IRandomAccessStream textStream = await newFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (DataWriter textWriter = new DataWriter(textStream))
                    {
                        textWriter.WriteString(ScriptPage.filecontent);
                        await textWriter.StoreAsync();
                    }
                }
                this.Content = null;
                ScriptPage.filename = newFile.Name.Split('.')[0];
                MessageBox.Show("file saved as "+newFile.Name);
            }
        }

        private void LayoutRoot_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }
    }
}
