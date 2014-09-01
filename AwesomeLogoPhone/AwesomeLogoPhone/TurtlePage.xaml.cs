using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Media;

namespace AwesomeLogoPhone
{
    public partial class TurtlePage : PhoneApplicationPage
    {
        public static string script;
        LogoTurtle LG;
        double scale = 0.6;

        public TurtlePage()
        {
            InitializeComponent();
            LG = new LogoTurtle(LayoutRoot, turtle_canvas, position_block);
            this.Loaded += TurtlePage_Loaded;
        }

        void TurtlePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (script != null)
            Prepare(script);
        }

        private void shell_box_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Enter"))
            {
                Prepare(shell_box.Text);

                if (LG.Message.Contains("Error"))
                {
                    MessageBox.Show(LG.Message, "Error", MessageBoxButton.OK);
                    shell_box.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                    shell_box.BorderBrush = shell_box.Background;
            }
        }

        private void Prepare(string script)
        {
            script = script.Replace("[", " [ ").Replace("]", " ] ").Replace("=", " = ");
            LG.Message = "";
            LG.Execute(script);
            LG.Update();
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            LG.ClearScreen();
            scale = 0.6;
            setScale(scale);
            LG.BgColorChange("orange");
            LG.Reset();
            LG.Update();
        }

        private void repeat_button_Click(object sender, EventArgs e)
        {
            if (script != null)
                Prepare(script);
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            Prepare(shell_box.Text);

            if (LG.Message.Contains("Error"))
            {
                MessageBox.Show(LG.Message, "Error", MessageBoxButton.OK);
                shell_box.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
                shell_box.BorderBrush = shell_box.Background;
        }

        private void setScale(double scale)
        {
            ScaleTransform sc = new ScaleTransform();
            sc.ScaleX = scale;
            sc.ScaleY = scale;
            turtle_canvas.RenderTransform = sc;

            ApplicationBarIconButton zib = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            ApplicationBarIconButton zob = (ApplicationBarIconButton)ApplicationBar.Buttons[3];
            zob.IsEnabled = (scale < 0.2) ? false : true;
            zib.IsEnabled = (scale < 2.9) ? true : false;
        }

        private void zoomin_button_Click(object sender, EventArgs e)
        {
            scale += 0.1;
            setScale(scale);
        }

        private void zoomout_button_Click(object sender, EventArgs e)
        {
            scale -= 0.1;
            setScale(scale);
        }

        //private void turtle_canvas_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        //{
        //    UIElement element = sender as UIElement;
        //    CompositeTransform transform = element.RenderTransform as CompositeTransform;
        //    if (transform != null)
        //    {
        //        transform.ScaleX *= e.DeltaManipulation.Scale.X;
        //        transform.ScaleY *= e.DeltaManipulation.Scale.Y;
        //        transform.TranslateX += e.DeltaManipulation.Translation.X;
        //        transform.TranslateY += e.DeltaManipulation.Translation.Y;
        //    }
        //}

        //private void image_OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        //{
        //    // the user has started manipulating the screen, set starting points
        //    var transform = (CompositeTransform)image.RenderTransform;
        //    _scaleX = transform.ScaleX;
        //    _scaleY = transform.ScaleY;
        //    _translationX = transform.TranslateX;
        //    _translationY = transform.TranslateY;
        //}
    }
}