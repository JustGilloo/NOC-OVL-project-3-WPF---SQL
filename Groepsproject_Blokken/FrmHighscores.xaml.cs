using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmHighscores.xaml
    /// </summary>
    public partial class FrmHighscores : Window
    {
        public Player ingelogdePlayerMainWindow = new Player();
        public MediaPlayer backgroundMusicPlayer { get; set; }

        public FrmHighscores()
        {
            InitializeComponent();
        }


        public List<Player> Players { get; set; }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CleanList();
            //ExcelWordStatic.PrintExcel(Players);
            AlleImagesLaden();
        }

        private void AlleImagesLaden() //Deze methode zorgt ervoor dat hij in playerplus deze images inlaad uit onze lokale map zodat iedereen elkaars pfp kan zien!
        {
            foreach (Player player in Players)
            {
                player.ImageInladenMetMemoryStream();
            }
        }

        private void CleanList()
        {
            Players = DataManager.GetAllPlayers();
            Players = Players.OrderByDescending(p => p.SPHighscore).ToList();
            Players = Players.Take(10).ToList();



            int index = 1;
            foreach (var player in Players)
            {
                player.Position = index++;
            }

            lstHighscores.ItemsSource = Players;


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

        private void btnReturn_Click_1(object sender, RoutedEventArgs e)
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
            StackPanelButtonsWeg2.Begin();
            BlokkenLogoTerug.Begin();

        }
    }
}
