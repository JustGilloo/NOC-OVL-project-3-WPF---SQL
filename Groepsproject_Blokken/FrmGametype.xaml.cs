using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmGametype.xaml
    /// </summary>
    public partial class FrmGametype : System.Windows.Window
    {
        public Player ingelogdePlayerMainWindow = new Player();
        public FrmGametype()
        {
            InitializeComponent();
        }
        // deze heb ik nodig voor de Primewords te laden en weg te schrijven
        PrimeWord myPrimeWord;
        string[] temperaryLines;
        string[,] wordsThatAreParted;
        int teller = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LaadTXTinCMB();
                ReadAndFillPrimeWordList();
                RandomPrimeWordConstruction();
            }
            catch
            {

            }
        }
        List<string> listIngeladenActieveVragenlijsten; //Txt's
        List<string> listGekozenVragenlijsten = new List<string>(); //*Leeg -> dit gaan we de selectie van de gebruiker inladen
        public MediaPlayer backgroundMusicPlayer { get; set; }
        private void btnSingle_Click(object sender, RoutedEventArgs e)
        {
            BerndCrabbeWeg.Completed += (s, args) =>
            {
                FrmSinglePlayerQuiz windowSP = new FrmSinglePlayerQuiz();
                windowSP.gekozenPrimeword = myPrimeWord;
                windowSP.ingelogdePlayer = ingelogdePlayerMainWindow;
                backgroundMusicPlayer.Stop();
                if (listGekozenVragenlijsten.Count == 0)
                {
                    MessageBox.Show("Alle vragen werden geladen omdat u geen topics heeft geselecteerd, succes!", "Melding", MessageBoxButton.OK, MessageBoxImage.Information);
                    windowSP.gekozenVragenLijsten = listIngeladenActieveVragenlijsten; // Laad alles in als niks geselecteerd is
                }
                else
                {
                    windowSP.gekozenVragenLijsten = listGekozenVragenlijsten;      // Als het niet null is, laden we  in wat er geselecteerd is.
                }

                this.Close();
                windowSP.ShowDialog();
            };

            BerndCrabbeWeg.Begin();

        }
        private void btnVS_Click(object sender, RoutedEventArgs e)
        {
            StackPanelButtonsWeg.Completed += (s, args) =>
            {
                FrmVersusQuizWindow windowVS = new FrmVersusQuizWindow();
                FrmLoginRegister frmLoginRegister = new FrmLoginRegister();
                frmLoginRegister.isLoginMultiplayer = true;
                frmLoginRegister.btnDoorgaanAlsGast.IsEnabled = false;
                backgroundMusicPlayer.Stop();
                frmLoginRegister.ShowDialog();
                windowVS.ingelogdePlayer2 = frmLoginRegister.ingelogdePlayer;
                windowVS.ingelogdePlayer1 = ingelogdePlayerMainWindow;
                if (listGekozenVragenlijsten.Count == 0)
                {
                    MessageBox.Show("Alle vragen werden geladen omdat u geen topics heeft geselecteerd, succes!", "Melding", MessageBoxButton.OK, MessageBoxImage.Information);
                    windowVS.gekozenVragenLijsten = listIngeladenActieveVragenlijsten; // Laad alles in als niks geselecteerd is
                }
                else
                {
                    windowVS.gekozenVragenLijsten = listGekozenVragenlijsten;      // Als het niet null is, laden we  in wat er geselecteerd is.
                }
                this.Close();
                windowVS.ShowDialog();
            };
            StackPanelButtonsWeg.Begin();
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StackPanelButtonsWeg.Completed += (s, args) =>
            {
                FrmTitleScreen window = new FrmTitleScreen();
                window.ingelogdePlayerLoginscreen = ingelogdePlayerMainWindow;
                window.backgroundMusicPlayer = backgroundMusicPlayer;
                window.sliderVolume.Value = backgroundMusicPlayer.Volume * 100;
                this.Close();
                window.ShowDialog();
            };
            StackPanelButtonsWeg.Begin();
            StackPanelVragenlijstenWeg.Begin();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

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
        private void LaadTXTinCMB()
        {

            listIngeladenActieveVragenlijsten = new List<string>(File.ReadAllLines("../../Questionaires/VragenlijstActief.txt"));
            cmbVragenLijsten.ItemsSource = listIngeladenActieveVragenlijsten;

        }
        private void btnVoegVragenLijstToe_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVragenLijsten.SelectedIndex != -1)
            {
                listGekozenVragenlijsten.Add(cmbVragenLijsten.SelectedItem.ToString());
                lbQuestionsDisplay.ItemsSource = null;
                lbQuestionsDisplay.ItemsSource = listGekozenVragenlijsten;

            }
        }
        public void ReadAndFillPrimeWordList() //i want to make a method that will fill in a 2d array of (prime)words, read from a txt
        {
            var filepath = "../../PrimeWords/List1.txt";
            try
            {
                StreamReader readobject = new StreamReader(filepath);
                if (!readobject.EndOfStream)
                {
                    while (!readobject.EndOfStream)
                    {
                        readobject.ReadLine();
                        //if (readobject.ToString() == null || readobject.ToString() == "") //this is when txt1 is empty
                        //{
                        //    File.Copy("../../PrimeWords/List2.txt", "../../PrimeWords/List1.txt");
                        //    System.IO.File.WriteAllText("../../PrimeWords/file2.txt", string.Empty);
                        //}
                        //else //here i just need the lenght of the list, so i need to increase "teller"
                        //{
                        teller++;
                        //}
                    }
                    readobject.Close();
                }
                else
                {
                    readobject.Close();
                    File.Delete("../../PrimeWords/List1.txt");
                    File.Copy("../../PrimeWords/List2.txt", "../../PrimeWords/List1.txt");
                    System.IO.File.WriteAllText("../../PrimeWords/List2.txt", string.Empty);
                    StreamReader readObjectNaLegeList = new StreamReader(filepath);
                    if (!readObjectNaLegeList.EndOfStream)
                    {
                        while (!readObjectNaLegeList.EndOfStream)
                        {
                            readObjectNaLegeList.ReadLine();
                            teller++;
                        }
                        readObjectNaLegeList.Close();
                    }

                }

                StreamReader readobject2 = new StreamReader(filepath);
                while (!readobject2.EndOfStream)
                {
                    temperaryLines = new string[teller]; //now that i have teller, i can make my length
                    for (int i = 0; i < teller; i++)
                    {
                        temperaryLines[i] = readobject2.ReadLine();
                    }
                }
                readobject2.Close();
                wordsThatAreParted = new string[teller, 2];
                for (int i = 0; i < teller; i++)
                {
                    string[] parts = temperaryLines[i].Split(';');
                    if (parts.Length >= 2)
                    {
                        wordsThatAreParted[i, 0] = parts[0];  // Left part this will be the primeword then
                        wordsThatAreParted[i, 1] = parts[1];  // Right part this will be the hint
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found. Please check the file path", "Error");
            }
        }
        public void RandomPrimeWordConstruction()
        {
            Random myRandom = new Random();
            var randomNumberToPickAndRemove = myRandom.Next(0, teller);
            myPrimeWord = new PrimeWord((wordsThatAreParted[randomNumberToPickAndRemove, 0]), (wordsThatAreParted[randomNumberToPickAndRemove, 1])); //here we have our random prime word with the random Hint
            string[,] writeAwayString = new string[teller - 1, 2]; //TODO: this one we can move to main, so we can get it for the writing for the new TXT
            for (int i = 0, j = 0; i < teller; i++) //ik wil hier een 2d string opvullen zonder het gekozen woord
            {
                if (myPrimeWord.Primeword.ToString() == wordsThatAreParted[i, 0])
                {
                    if (myPrimeWord.Hint.ToString() == wordsThatAreParted[i, 1])
                    {
                        continue;
                    }
                }
                else
                {
                    for (int k = 0, u = 0; k < wordsThatAreParted.GetLength(1); k++)
                    {
                        {
                            writeAwayString[j, u] = wordsThatAreParted[i, k];
                            u++;
                        }
                    }
                    j++;
                }
            }
            WriteAwayToTxtOne(writeAwayString);
            WriteAwayToTxtTwo(myPrimeWord);
        }
        public void WriteAwayToTxtOne(string[,] writeAwayString) //overschrijven List1 maar dan zonder de gekozen Primeword
        {
            var filepath = "../../PrimeWords/List1.txt";
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                for (int i = 0; i < writeAwayString.GetLength(0); i++) //we write away first the lines that we aleady can read
                {
                    writer.WriteLine(writeAwayString[i, 0] + ";" + writeAwayString[i, 1]);
                }
            }
        }
        public void WriteAwayToTxtTwo(PrimeWord wegschrijvenWoord) //ik heb de Primeword nodig
        {
            var filepath = "../../PrimeWords/List2.txt";
            string[] toWriteAway;
            int textLenght = 0;
            StreamReader readobject = new StreamReader(filepath);
            string temperaryLines;
            while (!readobject.EndOfStream)
            {
                readobject.ReadLine();
                textLenght++;
            }
            readobject.Close();
            StreamReader readobject2 = new StreamReader(filepath);
            toWriteAway = new string[textLenght];
            while (!readobject2.EndOfStream)
            {
                for (int i = 0; i < textLenght; i++)
                {
                    toWriteAway[i] = readobject2.ReadLine();
                }
            }
            readobject2.Close();
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                for (int i = 0; i < textLenght; i++)
                {
                    writer.WriteLine(toWriteAway[i]);
                }
                temperaryLines = wegschrijvenWoord.Primeword + ";" + wegschrijvenWoord.Hint;
                writer.WriteLine(temperaryLines); //we write away the newly used primeword
            }
        }
        //dit is de methode om een primeword te verwerken die zou moeten opgeroepen worden elke X seconden, en dan een letter meer zou moeten terug geven
        //deze verwerken de 2 objecten boven aan de page

    }
}