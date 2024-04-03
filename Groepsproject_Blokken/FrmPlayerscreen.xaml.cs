using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmPlayerscreen.xaml
    /// </summary>
    public partial class FrmPlayerscreen : Window
    {
        public Player ingelogdePlayerMainWindow = new Player();
        OpenFileDialog dialogChoosePicture = new OpenFileDialog
        {
            DefaultExt = "",
            Filter = "JPG files (*.jpg)|*.jpg|JPEG files(*.jpeg)|*.jpeg|PNG files (*.png)|*.png|All files (*.*)|*.*"
        };
        BitmapImage bmp = new BitmapImage();
        string profilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Profielfotos");
        string fileName = "";
        public MediaPlayer backgroundMusicPlayer { get; set; }

        public FrmPlayerscreen()
        {
            InitializeComponent();


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SpelerGegevensInladen();
            //Memory forcen de image in te laden 
            MemoryStream ms = new MemoryStream();
            FileStream stream = new FileStream(ingelogdePlayerMainWindow.ProfilePicture, FileMode.Open, FileAccess.Read);
            ms.SetLength(stream.Length);
            stream.Read(ms.GetBuffer(), 0, (int)stream.Length);
            ms.Flush();
            stream.Close();
            bmp.BeginInit();
            bmp.StreamSource = ms;
            bmp.EndInit();
            imgProfilePic.ImageSource = bmp;
            imgProfilePic.Stretch = System.Windows.Media.Stretch.UniformToFill;
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

            BerndCrabbeTerug.Completed += (s, args) =>
            {
                FrmTitleScreen window = new FrmTitleScreen();
                window.ingelogdePlayerLoginscreen = ingelogdePlayerMainWindow;
                window.backgroundMusicPlayer = backgroundMusicPlayer;
                window.sliderVolume.Value = backgroundMusicPlayer.Volume * 100;
                this.Close();
                window.ShowDialog();
            };
            BerndCrabbeTerug.Begin();
            StackPanelButtonsWeg.Begin();
            BlokkenLogoTerug.Begin();
            LinkseBorderTerug.Begin();
            RechtseBorderTerug.Begin();
        }

        private void btnWijzigWW_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnWijzigPFP_Click(object sender, RoutedEventArgs e)
        {
            if (dialogChoosePicture.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (dialogChoosePicture.FileName.Contains(".jpg") || dialogChoosePicture.FileName.Contains(".jpeg") || dialogChoosePicture.FileName.Contains(".png") || dialogChoosePicture.FileName.Contains(".PNG") || dialogChoosePicture.FileName.Contains(".JPG") || dialogChoosePicture.FileName.Contains(".JPEG"))
                {
                    ingelogdePlayerMainWindow.ProfilePicture = dialogChoosePicture.FileName;
                    imgProfilePic.ImageSource = new BitmapImage(new Uri(dialogChoosePicture.FileName, UriKind.RelativeOrAbsolute));
                    fileName = Path.GetFileName(dialogChoosePicture.FileName);
                    //Kopieren van de nieuwe pfp + gelijkstellen
                    if (!dialogChoosePicture.FileName.Contains("Groepsproject_Blokken\\Profielfotos"))
                    {
                        File.Copy(dialogChoosePicture.FileName, @"../../Profielfotos/" + fileName, true);
                    }
                    ingelogdePlayerMainWindow.ProfilePicture = @"../../Profielfotos/" + fileName;
                    DataManager.UpdatePlayer(ingelogdePlayerMainWindow);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("U heeft geen geldige afbeelding geselecteerd. Formaten jpg, jpeg en png worden ondersteund.", "Ongeldige selectie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            //Todo met tycho
            //ExcelWordStatic.PrintStatusSpeler(ingelogdePlayerMainWindow);
        }


        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            // YesButton Clicked -> Hide Inputvenster
            InputBox.Visibility = System.Windows.Visibility.Collapsed;

            //Verwerk data
            String input = InputTextBox.Text;
            ingelogdePlayerMainWindow.Password = InputTextBox.Text;
            DataManager.UpdatePlayer(ingelogdePlayerMainWindow); //Todo: Bevestiging in de datamanager?

            //Reset
            InputTextBox.Text = String.Empty;
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            // NoButton Clicked -> Hide box
            InputBox.Visibility = System.Windows.Visibility.Collapsed;

            //Reset
            InputTextBox.Text = String.Empty;
        }
        private void SpelerGegevensInladen()
        {
            lblNaamDisp.Content = ingelogdePlayerMainWindow.Name;
            lblGamesGespeeldSPDisp.Content = "0";
            lblGamesGespeeldVSDisp.Content = "0";
            lblWinrateSPDisp.Content = "0.00%";
            lblWinrateVSDisp.Content = "0.00%";
            if (ingelogdePlayerMainWindow.SPGamesPlayed != null)
            {
                lblGamesGespeeldSPDisp.Content = ingelogdePlayerMainWindow.SPGamesPlayed;
                lblWinrateSPDisp.Content = ingelogdePlayerMainWindow.CalculateSPWinRate();
                lblHighscoreSPDisp.Content = ingelogdePlayerMainWindow.SPHighscore;
            }
            if (ingelogdePlayerMainWindow.VSGamesPlayed != null)
            {
                lblGamesGespeeldVSDisp.Content = ingelogdePlayerMainWindow.VSGamesPlayed;
                lblHighscoreVSDisp.Content = ingelogdePlayerMainWindow.VSHighscore;
                lblWinrateVSDisp.Content = ingelogdePlayerMainWindow.CalculateVSWinRate();
            }
        }

        private void sliderVolume_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            sliderVolume.Opacity = 1;
            StackPanelVolume.Opacity = 1;
        }

        private void sliderVolume_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            sliderVolume.Opacity = 0.8;
            StackPanelVolume.Opacity = 0.4;
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderVolume != null && backgroundMusicPlayer != null)
            { backgroundMusicPlayer.Volume = sliderVolume.Value / 100; }
            if (imgVolume != null)
            {
                if (sliderVolume.Value == 0)
                {
                    imgVolume.Source = new BitmapImage(new Uri("Assets/Icon Mute.png", UriKind.Relative));
                }
                else if (sliderVolume.Value > 0 && sliderVolume.Value <= 33)
                {
                    imgVolume.Source = new BitmapImage(new Uri("Assets/Icon Low.png", UriKind.Relative));
                }
                else if (sliderVolume.Value > 33 && sliderVolume.Value <= 66)
                {
                    imgVolume.Source = new BitmapImage(new Uri("Assets/Icon Mid.png", UriKind.Relative));
                }
                else if (sliderVolume.Value > 66)
                {
                    imgVolume.Source = new BitmapImage(new Uri("Assets/Icon High.png", UriKind.Relative));
                }
            }
        }
    }
}
