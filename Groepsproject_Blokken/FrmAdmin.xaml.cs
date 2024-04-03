using System.Collections.Generic;
using System.Windows;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmAdmin.xaml
    /// </summary>
    public partial class FrmAdmin : Window
    {
        public FrmAdmin()
        {
            InitializeComponent();
            lijstManagers = DataManager.GetAllManagers();
            lstPlayers = DataManager.GetAllPlayers();
            lstManagers.ItemsSource = lijstManagers;
            lstManagers.DisplayMemberPath = "Name";

        }
        private List<Manager> lijstManagers = new List<Manager>();
        private List<Player> lstPlayers = new List<Player>();
        bool managerGevonden = false;

        private void btnCreateManager_Click(object sender, RoutedEventArgs e) //TODO: Extra databinding voor validatie overeenkomende paswoorden (voorlopig messagebox) + labels hiden
        {
            bool managerGevonden = false;
            Manager manager = new Manager();
            if (!(string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtPassword.Text) && string.IsNullOrEmpty(txtConfirmPassword.Text)))
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    manager.Name = txtUsername.Text;
                    foreach (Manager gelogdeManager in lijstManagers)
                    {
                        if (gelogdeManager.Equals(manager))
                        {
                            managerGevonden = true;
                        }
                    }
                    foreach (Player gelogdePLayer in lstPlayers)
                    {
                        if (gelogdePLayer.Name == manager.Name)
                        {
                            managerGevonden = true;
                        }
                    }
                    if (managerGevonden == true)
                    {
                        MessageBox.Show("Deze gebruikersnaam is al in gebruik, kies een andere gebruikersnaam", "Speler registreren", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else
                    {
                        manager.Password = txtPassword.Text;
                        if (DataManager.InsertManager(manager) == true)
                        {
                            MessageBox.Show("Manager succesvol aangemaakt!", "Manager aangemaakt", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Passwoord fout", "Passwoorden komen niet overeen.", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrmLoginRegister frmLoginRegister = new FrmLoginRegister();
            this.Close();
            frmLoginRegister.ShowDialog();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
