using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
//Todo: Met front end de lijst displayen en zorgen dat we deze kunnen crudden
namespace Groepsproject_Blokken
{
    /// <summary>
    /// Interaction logic for FrmManager.xaml
    /// </summary>
    public partial class FrmManager : Window
    {
        List<Question> tempquestions = new List<Question>(); //Opslagen van questions om later te editten
        Question question = new Question();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        bool fileIsLoaded = false;
        List<string> listAlleVragenlijsten;
        List<string> listActieveVragenlijsten;
        List<Player> listPlayers = new List<Player>();
        Player geselecteerdePlayer = new Player();
        bool updateAanList = false; //Nodig voor mn txtallevragenlijsten , omdat we dezelfde knop gebruiken om op te slagen en up te daten,  gaat hij bij elke update de vragenlijst opnieuw overschrijven
        List<string> listPrimewordsLst1;
        List<string> listPrimewordsLst2;
        string filepath1 = "../../PrimeWords/List1.txt";
        string filepath2 = "../../PrimeWords/List2.txt";
        string stringSubString;
        string primewordString;
        string hintString;
        string profilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Questionaires");
        public FrmManager()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LaadTxtsInListboxen();
            VulAlleVragenListBoxenIn();
            RefreshPlayers();
            LaadAllePrimeWords();
        }
        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtQuestion.Text) && string.IsNullOrEmpty(txtCorrectAnswer.Text) && string.IsNullOrEmpty(txtWrongAnswer1.Text) && string.IsNullOrEmpty(txtWrongAnswer2.Text) && string.IsNullOrEmpty(txtWrongAnswer3.Text)))
            {
                if (lbQuestions.SelectedIndex == -1) //enkel een nieuwe question aanmaken als er geen geselecteerd is, anders geselecteerde vraag aanpassen
                {
                    question = new Question();
                }
                question.TheQuestion = txtQuestion.Text;
                question.CorrectAnswer = txtCorrectAnswer.Text;
                question.WrongAnswerOne = txtWrongAnswer1.Text;
                question.WrongAnswerTwo = txtWrongAnswer2.Text;
                question.WrongAnswerThree = txtWrongAnswer3.Text;
                if (lbQuestions.Items.Count != 0 && lbQuestions.SelectedIndex != -1)
                {
                    foreach (Question q in tempquestions)
                    {
                        if (q.QuestionID == question.QuestionID)
                        {
                            q.TheQuestion = question.TheQuestion;
                            q.CorrectAnswer = question.CorrectAnswer;
                            q.WrongAnswerOne = question.WrongAnswerOne;
                            q.WrongAnswerTwo = question.WrongAnswerTwo;
                            q.WrongAnswerThree = question.WrongAnswerThree;
                            break;
                        }
                    }
                }
                else
                {
                    tempquestions.Add(question);
                }
                RefreshFields();
            }
        }
        private void btnSaveList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFileName.Text) && tempquestions.Count != 0)
                {
                    WegSchrijven(tempquestions, txtFileName.Text);
                    tempquestions.Clear();
                    fileIsLoaded = false;
                    OpslagenInVragenLijstAlles(txtFileName.Text);
                    RefreshFields();
                    System.Windows.MessageBox.Show("Vragenlijst succesvol opgeslagen!", "Vragen opgeslagen", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtFileName.IsEnabled = true;
                    VulAlleVragenListBoxenIn();
                    LaadTxtsInListboxen();
                    updateAanList = false;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Er ging iets mis. Het bestand is niet opgeslagen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            tempquestions.Remove((Question)lbQuestions.SelectedItem);
            RefreshFields();
        }
        private void btnLoadList_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = System.IO.Path.GetFullPath(profilePath),
                DefaultExt = "",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };
            tempquestions.Clear();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InlezenVragen(openFileDialog.FileName);
                fileIsLoaded = true;
                RefreshFields();
                txtFileName.Text = openFileDialog.SafeFileName.ToString();
                txtFileName.IsEnabled = false;
                updateAanList = true;
            }
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            FrmLoginRegister frmLoginRegister = new FrmLoginRegister();
            this.Close();
            frmLoginRegister.ShowDialog();
        }
        public void WegSchrijven(List<Question> eenList, string fileName)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(eenList, options);
            if (fileIsLoaded == true)
            {
                File.WriteAllText(openFileDialog.FileName, json);
            }
            else
            {
                File.WriteAllText("../../Questionaires/" + fileName, json);
            }
        }
        public void InlezenVragen(string pad)
        {
            using (StreamReader r = new StreamReader(pad))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                tempquestions.Clear();            // Lijst leegmaken
                string json = r.ReadToEnd(); // Tekst inlezen in een string
                tempquestions = JsonSerializer.Deserialize<List<Question>>(json); //De string inlezen en deserializen in een list, de properties moeten overeen komen.
            }
        }
        public void RefreshFields()
        {
            txtQuestion.Text = string.Empty;
            txtCorrectAnswer.Text = string.Empty;
            txtWrongAnswer1.Text = string.Empty;
            txtWrongAnswer2.Text = string.Empty;
            txtWrongAnswer3.Text = string.Empty;
            if (fileIsLoaded == false)
            {
                txtFileName.Text = "Filenaam";
                txtFileName.Foreground = Brushes.LightSlateGray;
                txtFileName.GotFocus += txtFileName_GotFocus;
            }
            lbQuestions.ItemsSource = null;
            lbQuestions.ItemsSource = tempquestions;
            lbQuestions.SelectedIndex = -1;
        }

        private void txtFileName_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            tb.Text = string.Empty;
            tb.Foreground = Brushes.Black;
            tb.GotFocus -= txtFileName_GotFocus;
        }

        private void lbQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbQuestions.SelectedIndex == -1)
            {
                btnAddToList.Content = "VRAAG AAN LIJST TOEVOEGEN";
                btnDeleteQuestion.Visibility = Visibility.Hidden;
            }
            if (lbQuestions.SelectedIndex != -1)
            {
                btnAddToList.Content = "VRAAG WIJZIGEN";
                btnDeleteQuestion.Visibility = Visibility.Visible;
                question = (Question)lbQuestions.SelectedItem;
                txtQuestion.Text = (string)question.TheQuestion;
                txtCorrectAnswer.Text = (string)question.CorrectAnswer;
                txtWrongAnswer1.Text = (string)question.WrongAnswerOne;
                txtWrongAnswer2.Text = (string)question.WrongAnswerTwo;
                txtWrongAnswer3.Text = (string)question.WrongAnswerThree;
            }
        }

        private void btnAddQuestionnaire_Click(object sender, RoutedEventArgs e)
        {
            var item = lbAllQuestionnaires.SelectedItem;
            listActieveVragenlijsten.Add(item.ToString());
            VulAlleVragenListBoxenIn();

        }

        private void btnReturnQuestionnaireSelection_Click(object sender, RoutedEventArgs e)
        {
            FrmLoginRegister frmLoginRegister = new FrmLoginRegister();
            this.Close();
            frmLoginRegister.ShowDialog();
        }

        private void btnSaveQuestionnaireSelection_Click(object sender, RoutedEventArgs e)
        {
            ActieveVragenLijstOpslagen();
        }
        private void OpslagenInVragenLijstAlles(string vraagTopic)
        {
            if (updateAanList == false)
            {
                string temp = "";
                using (StreamReader r = new StreamReader("../../Questionaires/VragenlijstAlles.txt"))
                {
                    temp = r.ReadToEnd();
                }
                using (StreamWriter w = new StreamWriter("../../Questionaires/VragenlijstAlles.txt"))
                {
                    w.Write(temp + Environment.NewLine + vraagTopic);
                }
            }
        }
        private void VulAlleVragenListBoxenIn()
        {
            lbAllQuestionnaires.ItemsSource = null;
            lbAllQuestionnaires.ItemsSource = listAlleVragenlijsten;
            lbActiveQuestionnaires.ItemsSource = null;
            lbActiveQuestionnaires.ItemsSource = listActieveVragenlijsten;
        }
        private void LaadTxtsInListboxen()
        {
            listAlleVragenlijsten = new List<string>(File.ReadAllLines("../../Questionaires/VragenlijstAlles.txt"));
            listActieveVragenlijsten = new List<string>(File.ReadAllLines("../../Questionaires/VragenlijstActief.txt"));
        }
        private void ActieveVragenLijstOpslagen()
        {
            using (StreamWriter wr = new StreamWriter("../../Questionaires/VragenlijstActief.txt"))
            {
                foreach (var item in lbActiveQuestionnaires.Items)
                {
                    wr.WriteLine(item.ToString());
                }
            }
        }

        private void btnDeleteQuestionnaire_Click(object sender, RoutedEventArgs e)
        {
            if (lbActiveQuestionnaires.SelectedIndex != -1)
            {
                var selectedItem = lbActiveQuestionnaires.SelectedItem;
                foreach (var item in listActieveVragenlijsten)
                {
                    if (selectedItem.ToString() == item.ToString())
                    {
                        listActieveVragenlijsten.Remove(item);
                        break;
                    }
                }
                VulAlleVragenListBoxenIn();
            }
        }

        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayerdisp.SelectedIndex != -1)
            {
                geselecteerdePlayer = lbPlayerdisp.SelectedItem as Player;
                DataManager.DeletePlayer(geselecteerdePlayer);
                RefreshPlayers();
            }
        }

        private void btnResetWW_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayerdisp.SelectedIndex != -1)
            {
                geselecteerdePlayer = lbPlayerdisp.SelectedItem as Player;
                geselecteerdePlayer.Password = "default";
                DataManager.UpdatePlayer(geselecteerdePlayer);
                RefreshPlayers();

            }
        }

        private void btnResetSP_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayerdisp.SelectedIndex != -1)
            {
                geselecteerdePlayer = lbPlayerdisp.SelectedItem as Player;
                geselecteerdePlayer.SPGamesPlayed = 0;
                geselecteerdePlayer.SPGamesWon = 0;
                DataManager.UpdatePlayer(geselecteerdePlayer);
                RefreshPlayers();
            }
        }

        private void btnResetVS_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayerdisp.SelectedIndex != -1)
            {
                geselecteerdePlayer = lbPlayerdisp.SelectedItem as Player;
                geselecteerdePlayer.VSGamesPlayed = 0;
                geselecteerdePlayer.VSGamesWon = 0;
                DataManager.UpdatePlayer(geselecteerdePlayer);
                RefreshPlayers();
            }
        }
        private void RefreshPlayers()
        {
            listPlayers = DataManager.GetAllPlayers();
            lbPlayerdisp.ItemsSource = null;
            lbPlayerdisp.ItemsSource = listPlayers;
        }

        private void lbPrimeworddispay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbPrimeworddispay.SelectedIndex != -1)
            {
                stringSubString = "";
                lbPrimeworddisplay2.SelectedIndex = -1;
                stringSubString = lbPrimeworddispay.SelectedItem.ToString();
                txtPrimewordCrud.Text = stringSubString.Substring(0, 8);
                txtHintCrud.Text = stringSubString.Substring(9);
            }
            else
            {
                txtHintCrud.Text = "";
                txtPrimewordCrud.Text = "";
            }
        }

        private void lbPrimeworddisplay2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbPrimeworddisplay2.SelectedIndex != -1)
            {
                stringSubString = "";
                lbPrimeworddispay.SelectedIndex = -1;
                stringSubString = lbPrimeworddisplay2.SelectedItem.ToString();
                txtPrimewordCrud.Text = stringSubString.Substring(0, 8);
                txtHintCrud.Text = stringSubString.Substring(9);
            }
            else
            {
                txtHintCrud.Text = "";
                txtPrimewordCrud.Text = "";
            }
        }

        private void btnPrimewordToevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (!(txtPrimewordCrud.Text == " " || txtPrimewordCrud.Text == null))
            {
                if (!(txtHintCrud.Text == " " || txtHintCrud.Text == null))
                {
                    if (!(txtPrimewordCrud.Text.Contains(" ")))
                    {
                        if ((txtPrimewordCrud.Text.Count() == 8))
                        {
                            listPrimewordsLst1.Add(txtPrimewordCrud.Text + ";" + txtHintCrud.Text);
                            ListboxPrimewordsRefresh();
                        }
                        else System.Windows.MessageBox.Show("Het primeword moet 8 karakters lang zijn.", "Primeword validatie");
                    }
                    else System.Windows.MessageBox.Show("Het primeword mag niet leeg zijn.", "Primeword validatie");
                }
                else System.Windows.MessageBox.Show("De hint moet een waarde bevatten.", "Primeword validatie");
            }
            else System.Windows.MessageBox.Show("Het primeword moet een waarde bevatten.", "Primeword validatie");
        }

        private void btnPrimeWordVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (lbPrimeworddispay.SelectedIndex != -1)
            {
                listPrimewordsLst1.Remove(txtPrimewordCrud.Text + ";" + txtHintCrud.Text);
                ListboxPrimewordsRefresh();
            }
            else if (lbPrimeworddisplay2.SelectedIndex != -1)
            {
                listPrimewordsLst2.Remove(txtPrimewordCrud.Text + ";" + txtHintCrud.Text);
                ListboxPrimewordsRefresh();
            }
            else
            {
                System.Windows.MessageBox.Show("Select a Primeword to delete first", "Warning");
            }
        }

        private void btnPrimewordAllesResetten_Click(object sender, RoutedEventArgs e)
        {
            foreach (var woorden in listPrimewordsLst2)
            {
                listPrimewordsLst1.Add(woorden + Environment.NewLine);
            }
            using (StreamWriter schrijver1 = new StreamWriter(filepath1))
            {
                for (int i = 0; i < listPrimewordsLst1.Count; i++)
                {
                    schrijver1.WriteLine(listPrimewordsLst1[i]);
                }
            }
            System.IO.File.WriteAllText("../../PrimeWords/file2.txt", string.Empty);
            listPrimewordsLst2.Clear();
        }
        private void ListboxPrimewordsRefresh()
        {
            lbPrimeworddispay.ItemsSource = null;
            lbPrimeworddispay.ItemsSource = listPrimewordsLst1;
            lbPrimeworddisplay2.ItemsSource = null;
            lbPrimeworddisplay2.ItemsSource = listPrimewordsLst2;
        }
        private void LaadAllePrimeWords()
        {
            try
            {
                listPrimewordsLst1 = new List<string>(File.ReadAllLines(filepath1));
                listPrimewordsLst2 = new List<string>(File.ReadAllLines(filepath2));
                lbPrimeworddispay.ItemsSource = null;
                lbPrimeworddispay.ItemsSource = listPrimewordsLst1;
                lbPrimeworddisplay2.ItemsSource = null;
                lbPrimeworddisplay2.ItemsSource = listPrimewordsLst2;
            }
            catch
            {
                System.Windows.MessageBox.Show("Files not found. Please check the file directory", "Error");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WegschrijvenPrimeWordLists();
        }
        private void WegschrijvenPrimeWordLists()
        {
            using (StreamWriter streamWriter = new StreamWriter(filepath1)) //schrijven voor file 1
            {
                for (int i = 0; i < listPrimewordsLst1.Count; i++)
                {
                    streamWriter.WriteLine(listPrimewordsLst1[i]);
                }
            }
            using (StreamWriter streamWriter = new StreamWriter(filepath2))
            {
                for (int i = 0; (i < listPrimewordsLst2.Count); i++)
                {
                    streamWriter.WriteLine(listPrimewordsLst2[i]);
                }
            }
        }
    }
}
