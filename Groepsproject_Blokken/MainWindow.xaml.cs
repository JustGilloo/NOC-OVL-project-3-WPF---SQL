using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Player ingelogdePlayerLoginscreen = new Player();
        public MainWindow()
        {
            InitializeComponent();
            PlaybackMusic();
        }
        MediaPlayer backgroundMusicPlayer = new MediaPlayer();
        public void PlaybackMusic()
        {
            string backgroundMusicFilePath = "../../Assets/Blokken PlaceHolderLoop.wav";
            if (!string.IsNullOrEmpty(backgroundMusicFilePath))
            {
                backgroundMusicPlayer.Open(new Uri(backgroundMusicFilePath, UriKind.Relative));
                backgroundMusicPlayer.MediaEnded += new EventHandler(Media_Ended);

                backgroundMusicPlayer.Play();
            }
        }
        private void Media_Ended(object sender, EventArgs e)
        {
            backgroundMusicPlayer.Position = TimeSpan.Zero;
            backgroundMusicPlayer.Play();
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            StackPanelButtonsWeg.Completed += (s, args) =>
            {
                FrmGametype chooseGame = new FrmGametype();
                chooseGame.ingelogdePlayerMainWindow = ingelogdePlayerLoginscreen;
                chooseGame.backgroundMusicPlayer = backgroundMusicPlayer;
                chooseGame.sliderVolume.Value = backgroundMusicPlayer.Volume * 100;
                this.Close();
                chooseGame.ShowDialog();
            };

            StackPanelButtonsWeg.Begin();
        }

        private void btnHighscore_Click(object sender, RoutedEventArgs e)
        {
            FrmHighscores frmHighscores = new FrmHighscores();
            frmHighscores.ingelogdePlayerMainWindow = ingelogdePlayerLoginscreen;
            frmHighscores.backgroundMusicPlayer = backgroundMusicPlayer;
            frmHighscores.sliderVolume.Value = backgroundMusicPlayer.Volume * 100;
            this.Close();
            frmHighscores.ShowDialog();

        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            FrmPlayerscreen frmPlayerscreen = new FrmPlayerscreen();
            frmPlayerscreen.ingelogdePlayerMainWindow = ingelogdePlayerLoginscreen;
            frmPlayerscreen.backgroundMusicPlayer = backgroundMusicPlayer;
            frmPlayerscreen.sliderVolume.Value = backgroundMusicPlayer.Volume * 100;
            this.Close();
            frmPlayerscreen.ShowDialog();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            FrmLoginRegister frmLoginRegister = new FrmLoginRegister();
            backgroundMusicPlayer.Stop();
            this.Close();
            frmLoginRegister.ShowDialog();
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
            backgroundMusicPlayer.Volume = sliderVolume.Value / 100;
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


