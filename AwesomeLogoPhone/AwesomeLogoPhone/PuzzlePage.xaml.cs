using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AwesomeLogoPhone
{
    public partial class PuzzlePage : PhoneApplicationPage
    {
        int curPuzzle, maxPuzzle, solvedPuzzle;
        string userScript, puzzleScript, hintScript;
        LogoTurtle PLG, ULG, HLG;
        
        public PuzzlePage()
        {
            this.InitializeComponent();

            solvedPuzzle = 0;

            PLG = new LogoTurtle(LayoutRoot, puzzle_canvas, position_block);
            ULG = new LogoTurtle(LayoutRoot, puzzle_canvas, position_block);
            HLG = new LogoTurtle(LayoutRoot, puzzle_canvas, null);

            PLG.Update();
            ULG.Update();
            HLG.Update();

            this.Loaded += PuzzlePage_Loaded;
        }

        private async void PuzzlePage_Loaded(object sender, RoutedEventArgs e)
        {
            var countfile = Application.GetResourceStream(new Uri("Puzzles/Count.txt", UriKind.Relative));
            StreamReader streamReader1 = new StreamReader(countfile.Stream);
            string data1 = streamReader1.ReadToEnd();
            maxPuzzle = int.Parse(data1);

            try
            {
                var solvedfile = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("Solved.txt");
                using (StreamReader streamReader = new StreamReader(solvedfile))
                {
                    solvedPuzzle = int.Parse("0" + streamReader.ReadToEnd());
                }
            }
            catch(Exception ex){
                solvedPuzzle = 0;
                update_Progress(0);
            }

            if (curPuzzle < maxPuzzle)
                curPuzzle = solvedPuzzle + 1;

            ReadyPuzzle();
        }

        private void ReadyPuzzle()
        {
            // Read Puzzle file
            var puzzlefile = Application.GetResourceStream(new Uri("Puzzles/Scripts/Puzzle"+curPuzzle+".txt", UriKind.Relative));
            StreamReader puzzleReader = new StreamReader(puzzlefile.Stream);
            string puzzledata = puzzleReader.ReadToEnd();
       
            puzzledata = puzzledata.Replace("[", " [ ").Replace("]", " ] ").Replace("=", " = ");

            char[] ch = { ' ', '\n' };
            var words = puzzledata.Split(ch, StringSplitOptions.RemoveEmptyEntries);
            
            puzzleScript = "";
            foreach (string word in words)
            {
                puzzleScript += word + " ";
            }
        
            // Read Hint file
            var hintfile = Application.GetResourceStream(new Uri("Puzzles/Hints/Hint" + curPuzzle + ".txt", UriKind.Relative));
            StreamReader hintReader = new StreamReader(hintfile.Stream);
            string hintdata = hintReader.ReadToEnd();
            
            hintdata = hintdata.Replace("[", " [ ").Replace("]", " ] ").Replace("=", " = ");

            words = hintdata.Split(ch, StringSplitOptions.RemoveEmptyEntries);

            hintScript = "";
            foreach (string word in words)
            {
                 hintScript += word + " ";
            }
            
            //start puzzle
            RestartPuzzle();
        }

        private void RestartPuzzle()
        {
            title_block.Text = "puzzle #" + curPuzzle;

            userScript = "";

            PLG.ClearScreen();
            PLG.Reset();
            ULG.Reset();
            HLG.Reset();

            PLG.Execute("pc white " + puzzleScript);
            HLG.Execute("pc black ht pu home " + hintScript + " ht");
            ULG.Execute("pc black");

            PLG.Update();
            HLG.Update();
            ULG.Update();

            //enable or disable icon buttons
            ApplicationBarIconButton prev_b = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            ApplicationBarIconButton next_b = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            prev_b.IsEnabled = (curPuzzle == 1) ? false : true; ;
            next_b.IsEnabled = (curPuzzle < maxPuzzle && curPuzzle <= solvedPuzzle) ? true : false;
        }

        private void prev_button_Click(object sender, EventArgs e)
        {
            if (curPuzzle > 1)
            {
                curPuzzle--;
                ReadyPuzzle();
            }
        }

        private void tryagain_button_Click(object sender, EventArgs e)
        {
            RestartPuzzle();
        }

        private void next_button_Click(object sender, EventArgs e)
        {
            if (curPuzzle < maxPuzzle && curPuzzle <= solvedPuzzle)
            {
                curPuzzle++;
                ReadyPuzzle();
            }
        }

        private void CheckScript()
        {
            if (userScript.Equals(puzzleScript, StringComparison.CurrentCultureIgnoreCase))
            {
                //save progress
                if (curPuzzle > solvedPuzzle && curPuzzle < maxPuzzle)
                {
                    solvedPuzzle = curPuzzle;
                    update_Progress(solvedPuzzle);
                    //update_Tile();
                }
                //show message
                MessageBoxResult mr = MessageBox.Show("You solved the puzzle, Keep it up.\nDo you want to play next puzzle?", "Congratulations!", MessageBoxButton.OKCancel);
                if (mr == MessageBoxResult.OK)
                    next_button_Click(new object(), null);
            }
        }

        private void shell_box_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Enter"))
            {
                string script = shell_box.Text;
                PrepareCode(script);
                CheckScript();
            }
        }

        private void help_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have to write the equivalnet code to trace the path drawn by the white turtle.\n" +
                           "As you write the code, the yellow turtle will move along the white path.\n" +
                           "Length of the lines are written besides them.", "Help", MessageBoxButton.OK);
        }

        private  async void update_Progress(int solved)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Solved.txt", CreationCollisionOption.OpenIfExists);
            using (IRandomAccessStream textStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (DataWriter textWriter = new DataWriter(textStream))
                {
                    textWriter.WriteString(""+solved);
                    await textWriter.StoreAsync();
                }
            }
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            string script = shell_box.Text;
            PrepareCode(script);
            CheckScript();
        }

        private void PrepareCode(string script)
        {
            script = script.Replace("[", " [ ").Replace("]", " ] ").Replace("=", " = ");

            ULG.Message = "";
            ULG.Execute(script);
            ULG.Update();

            if (ULG.Message.Contains("Error"))
            {
                shell_box.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show(ULG.Message, "Error", MessageBoxButton.OK);
            }
            else
            {
                //update userscript
                userScript += " " + script;
                char[] ch = { ' ', '\n' };
                var words = userScript.Split(ch, StringSplitOptions.RemoveEmptyEntries);

                userScript = "";
                foreach (string word in words)
                    userScript += word + " ";

                shell_box.BorderBrush = shell_box.Background;
            }
        }

        //private void update_Tile()
        //{
        //    //Tile Content
        //    string tileXmlString = String.Format(
        //        "<tile>"
        //        + "<visual version='2'>"
        //        + "<binding template='TileSquare150x150Text04' fallback='TileSquareText04'>"
        //        + "<text id='1'>Congratulations!\n\nPuzzle {0} unlocked.</text>"
        //        + "</binding>"
        //        + "<binding template='TileWide310x150Text03' fallback='TileWideText03'>"
        //        + "<text id='1'>Congratulations!\nPuzzle {0} unlocked.</text>"
        //        + "</binding>"
        //        + "<binding template='TileSquare310x310Text09'>"
        //        + "<text id='1'>Congratulations! You have unlocked puzzle {0}.</text>"
        //        + "</binding>"
        //        + "</visual>"
        //        + "</tile>",
        //        solvedPuzzle + 1);

        //    // Create a DOM. 
        //    Windows.Data.Xml.Dom.XmlDocument tileDOM = new Windows.Data.Xml.Dom.XmlDocument();

        //    // Load the xml string into the DOM. 
        //    tileDOM.LoadXml(tileXmlString);

        //    //clear all notifications
            
        //    TileUpdateManager.CreateTileUpdaterForApplication().Clear();

        //    //Create a tile notification. 
        //    TileNotification tile = new TileNotification(tileDOM);

        //    // Send the notification to the application's tile. 
        //    TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);
        //}
    }
}