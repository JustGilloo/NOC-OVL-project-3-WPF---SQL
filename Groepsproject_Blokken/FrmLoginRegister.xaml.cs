using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmLoginRegister.xaml
    /// </summary>
    public partial class FrmLoginRegister : Window
    {
        public FrmLoginRegister()
        {
            InitializeComponent();

        }
        BrushConverter bc = new BrushConverter();
        List<Player> playerList = new List<Player>();
        List<Manager> managerList = new List<Manager>();
        List<Admin> adminList = new List<Admin>();
        public Player ingelogdePlayer = new Player();
        Manager ingelogdeManager = new Manager();
        Admin ingelogdeAdmin = new Admin();
        bool adminGevonden = false;
        bool managerGevonden = false;
        bool playerGevonden = false;
        public bool isLoginMultiplayer = false;
        //TIJDELIJKE INLOGS/FAILSAFES DIE HARDCODED STAAN, DEZE ZOUDE WEG MOGEN LATER OF WE KUNNEN ZE LATEN STAAN INCASE IETS BREEKT
        Admin tempAdmin = new Admin();
        Manager tempManager = new Manager();
        Player tempPlayer = new Player();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TIJDELIJKE INLOGS/FAILSAFES DIE HARDCODED STAAN, DEZE ZOUDE WEG MOGEN LATER OF WE KUNNEN ZE LATEN STAAN INCASE IETS BREEKT
            tempAdmin.Name = "tAdmin";
            tempAdmin.Password = "tAdmin";
            tempManager.Name = "tManager";
            tempManager.Password = "tManager";
            tempPlayer.Name = "tPlayer";
            tempPlayer.Password = "tPlayer";
            //Inladen van db/json 

            playerList = DataManager.GetAllPlayers();
            managerList = DataManager.GetAllManagers();
            adminList = DataManager.GetAllAdmins();

        }
        private void btnAanmelden_Click(object sender, RoutedEventArgs e)
        {
            //inlogsysteem voor failsafe/hardcoded tempAdmin/Manager/Speler (TIJDELIJK)
            //LoginAlsTempGebruiker();

            //eerste attempt om een login systeem te maken voor met json/db
            VindIngelogdeGebruiker();
        }


        private void btnRegistreren_Click(object sender, RoutedEventArgs e)
        {
            FrmPlayerRegister frmPlayerRegister = new FrmPlayerRegister();
            frmPlayerRegister.lstPlayers = playerList;
            frmPlayerRegister.lstManagers = managerList;
            frmPlayerRegister.ShowDialog();
        }

        private void btnDoorgaanAlsGast_Click(object sender, RoutedEventArgs e)
        {
            List<Question> tempLijstVragen = new List<Question>();
            FrmSinglePlayerQuiz frmQuizWindow = new FrmSinglePlayerQuiz();
            using (StreamReader r = new StreamReader("../../Questionaires/Actua2023"))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                tempLijstVragen.Clear();
                string json = r.ReadToEnd(); // Tekst inlezen in een string
                tempLijstVragen = JsonSerializer.Deserialize<List<Question>>(json);
            }
            frmQuizWindow.finalLijstVragen = tempLijstVragen;
            frmQuizWindow.gekozenPrimeword = new PrimeWord("ingekort", "demo");
            this.Close();
            frmQuizWindow.ShowDialog();
        }

        private void btnAanmelden_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAanmelden.Background = (Brush)bc.ConvertFrom("#002266");
        }

        private void btnRegistreren_MouseEnter(object sender, MouseEventArgs e)
        {

            btnRegistreren.Background = (Brush)bc.ConvertFrom("#002266");
        }

        private void btnDoorgaanAlsGast_MouseEnter(object sender, MouseEventArgs e)
        {
            btnDoorgaanAlsGast.Background = (Brush)bc.ConvertFrom("#002266");

        }

        private void btnDoorgaanAlsGast_MouseLeave(object sender, MouseEventArgs e)
        {
            btnDoorgaanAlsGast.Background = (Brush)bc.ConvertFrom("#fea702");

        }

        private void btnRegistreren_MouseLeave(object sender, MouseEventArgs e)
        {
            btnRegistreren.Background = (Brush)bc.ConvertFrom("#fea702");

        }

        private void btnAanmelden_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAanmelden.Background = (Brush)bc.ConvertFrom("#fea702");

        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {

            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.Foreground = Brushes.Black;
            tb.GotFocus -= txtUsername_GotFocus;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            pb.Password = string.Empty;
            pb.Foreground = Brushes.Black;
            pb.GotFocus -= txtPassword_GotFocus;
        }
        //Methode IDEE voor inte loggen
        private void VindIngelogdeGebruiker()
        {
            //Ik heb 3 bools toegevoegd om niet altijd de iteratie moeten uit te voer
            if (txtUsername.Text == "Hanzie" && txtPassword.Password == "Hanzie") // Hans gamemode
            {
                List<Question> tempLijstVragen = new List<Question>();
                FrmSinglePlayerQuiz frmQuizWindow = new FrmSinglePlayerQuiz();
                using (StreamReader r = new StreamReader("../../Questionaires/HansVragenlijst"))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    tempLijstVragen.Clear();
                    string json = r.ReadToEnd(); // Tekst inlezen in een string
                    tempLijstVragen = JsonSerializer.Deserialize<List<Question>>(json);
                }
                frmQuizWindow.finalLijstVragen = tempLijstVragen;
                frmQuizWindow.gekozenPrimeword = new PrimeWord("ingekort", "demo");
                frmQuizWindow.hansMode = true;
                this.Close();
                frmQuizWindow.ShowDialog();
                adminGevonden = true;
                managerGevonden = true;
                playerGevonden = true;

            }
            if (adminGevonden == false)
            {
                foreach (Admin admin in adminList)
                {
                    if (admin.Name == txtUsername.Text && admin.Password == txtPassword.Password)
                    {
                        ingelogdeAdmin = DataManager.GetLoggedInAdmin(txtUsername.Text, txtPassword.Password);
                        adminGevonden = true;
                        managerGevonden = true;
                        playerGevonden = true;
                        FrmAdmin frmAdmin = new FrmAdmin();
                        this.Close();
                        frmAdmin.ShowDialog();
                    }
                }
            }
            if (managerGevonden == false)
            {
                foreach (Manager manager in managerList)
                {
                    if (manager.Name == txtUsername.Text && manager.Password == txtPassword.Password)
                    {
                        ingelogdeManager = DataManager.GetLoggedInManager(txtUsername.Text, txtPassword.Password);
                        managerGevonden = true;
                        playerGevonden = true;
                        FrmManager frmManager = new FrmManager();
                        this.Close();
                        frmManager.ShowDialog();
                    }
                }
            }
            if (playerGevonden == false)
            {
                foreach (Player player in playerList)
                {
                    if (player.Name == txtUsername.Text && player.Password == txtPassword.Password)
                    {
                        ingelogdePlayer = DataManager.GetLoggedInPlayer(txtUsername.Text, txtPassword.Password);
                        playerGevonden = true;
                        if (isLoginMultiplayer == false)
                        {
                            MainWindow mainwindow = new MainWindow();
                            mainwindow.ingelogdePlayerLoginscreen = ingelogdePlayer;
                            this.Close();
                            mainwindow.ShowDialog();
                        }
                        else
                        {
                            this.Hide();
                        }
                    }
                }
            }
            if (playerGevonden == false && managerGevonden == false && adminGevonden == false)
            {
                MessageBox.Show("Gebruiker werd niet gevonden!", "Gebruiker niet gevonden", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //reset bools voor volgende login
            adminGevonden = false;
            managerGevonden = false;
            playerGevonden = false;
        }
        //(TIJDELIJKE) Methode voor te kunnen inloggen zonder json gedoe
        private void LoginAlsTempGebruiker()
        {
            if (tempAdmin.Name == txtUsername.Text && tempAdmin.Password == txtPassword.Password)
            {
                FrmAdmin frmAdmin = new FrmAdmin();
                this.Close();
                frmAdmin.ShowDialog();
            }
            else if (tempManager.Name == txtUsername.Text && tempManager.Password == txtPassword.Password)
            {
                FrmManager frmManager = new FrmManager();
                this.Close();
                frmManager.ShowDialog();
            }
            else if (tempPlayer.Name == txtUsername.Text && tempPlayer.Password == txtPassword.Password)
            {
                MainWindow mainwindow = new MainWindow();
                this.Close();
                mainwindow.ShowDialog();
            }

        }


    }
}
