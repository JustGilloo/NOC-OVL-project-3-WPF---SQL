using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmPlayerRegister.xaml
    /// </summary>
    public partial class FrmPlayerRegister : Window
    {

        public List<Player> lstPlayers = new List<Player>();
        public List<Manager> lstManagers = new List<Manager>();
        BitmapImage defaultProfile = new BitmapImage(new Uri(@"../../Profielfotos/default.jpg", UriKind.RelativeOrAbsolute));
        string profilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Profielfotos"); //Path voor de initdirectory te laten werken
        string fileName = ""; // variabele om de naam van de file in op te slagen
        Player player = new Player();


        OpenFileDialog dialogChoosePicture = new OpenFileDialog
        {
            DefaultExt = "",
            Filter = "JPG files (*.jpg)|*.jpg|JPEG files(*.jpeg)|*.jpeg|PNG files (*.png)|*.png|All files (*.*)|*.*"
        };
        public FrmPlayerRegister()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProfilePicturePreview.Source = defaultProfile;
            txtProfilePictureSource.Text = defaultProfile.UriSource.ToString();
            dialogChoosePicture.InitialDirectory = System.IO.Path.GetFullPath(profilePath);
            player.ProfilePicture = @"../../Profielfotos/default.jpg";
        }

        private void btnSelectProfilePic_Click(object sender, RoutedEventArgs e)
        {
            if (dialogChoosePicture.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (dialogChoosePicture.FileName.Contains(".jpg") || dialogChoosePicture.FileName.Contains(".jpeg") || dialogChoosePicture.FileName.Contains(".png") || dialogChoosePicture.FileName.Contains(".PNG") || dialogChoosePicture.FileName.Contains(".JPG") || dialogChoosePicture.FileName.Contains(".JPEG"))
                {
                    fileName = "";
                    txtProfilePictureSource.Text = dialogChoosePicture.FileName;
                    defaultProfile = new BitmapImage(new Uri(dialogChoosePicture.FileName, UriKind.RelativeOrAbsolute));
                    txtProfilePicturePreview.Source = defaultProfile;
                    fileName = Path.GetFileName(dialogChoosePicture.FileName); //opslagen filename
                    if (!dialogChoosePicture.FileName.Contains("Groepsproject_Blokken\\Profielfotos"))
                    {
                        File.Copy(dialogChoosePicture.FileName, @"../../Profielfotos/" + fileName, true); // kopieer de lokale file naar onze profielfotosmap, als de foto al bestaat, gewoon overschrijven 
                    }
                    player.ProfilePicture = @"../../Profielfotos/" + fileName;

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("U heeft geen geldige afbeelding geselecteerd. Formaten jpg, jpeg en png worden ondersteund.", "Ongeldige selectie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCreatePlayer_Click(object sender, RoutedEventArgs e)
        {
            bool playerBestaat = false;
            if (!(string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtPassword.Text) && string.IsNullOrEmpty(txtConfirmPassword.Text)))
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    player.Name = txtUsername.Text;
                    foreach (Player loggedPlayer in lstPlayers)
                    {
                        if (loggedPlayer.Equals(player))
                        {
                            playerBestaat = true;
                            System.Windows.Forms.MessageBox.Show("Deze gebruikersnaam is al in gebruik, kies een andere gebruikersnaam", "Speler registreren", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        foreach (Manager loggedManager in lstManagers)
                        {
                            if (loggedManager.Name == player.Name)
                            {
                                playerBestaat = true;
                                System.Windows.Forms.MessageBox.Show("Deze gebruikersnaam is al in gebruik, kies een andere gebruikersnaam", "Speler registreren", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    if (playerBestaat == false)
                    {
                        player.Password = txtPassword.Text;
                        if (DataManager.InsertPlayer(player) == true)
                        {
                            System.Windows.Forms.MessageBox.Show("Speler succesvol aangemaakt!", "Speler registreren", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lstPlayers.Add(player);
                            this.Close();
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("De speler werd niet aangemaakt", "Speler registreren", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("De ingegeven wachtwoorden moeten overeenkomen", "Wachtwoord bevestigen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("U heeft niet alle benodigde velden (correct) ingevuld.", "Onvolledig formulier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
