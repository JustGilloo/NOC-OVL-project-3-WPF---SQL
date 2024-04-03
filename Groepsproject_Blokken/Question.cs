using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
//TODO: (nog) niks
namespace Groepsproject_Blokken
{
    public class Question : INotifyPropertyChanged, IDataErrorInfo
    {
        //Attributen
        private string _question;
        private string _correctAnswer;
        private string _wrongAnswerOne;
        private string _wrongAnswerTwo;
        private string _wrongAnswerThree;
        private int _questionDisplayTime;
        private DispatcherTimer _timer;
        private static int _questioncounter = 0;
        private int _questionID;



        //Constr
        [JsonConstructor]
        public Question()
        {
            _questioncounter += 1;
            QuestionID = _questioncounter;
        }
        public Question(string aQuestion, string aCorrectAnswer, string aWrongAnswerOne, string aWrongAnswerTwo, string aWrongAnswerThree)
        {
            _questioncounter += 1;
            QuestionID = _questioncounter;
            TheQuestion = aQuestion;
            CorrectAnswer = aCorrectAnswer;
            WrongAnswerOne = aWrongAnswerOne;
            WrongAnswerTwo = aWrongAnswerTwo;
            WrongAnswerThree = aWrongAnswerThree;
            QuestionDisplayTime = 10;
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
        }
        //Properties in volgorde , met het juiste antwoord als eerste
        public int QuestionID
        {
            get { return _questionID; }
            set { _questionID = value; }
        }
        public string TheQuestion
        {
            get { return _question; }
            set { OnPropertyChanged(ref _question, value); }
        }
        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set { OnPropertyChanged(ref _correctAnswer, value); }
        }
        public string WrongAnswerOne
        {
            get { return _wrongAnswerOne; }
            set { OnPropertyChanged(ref _wrongAnswerOne, value); }
        }
        public string WrongAnswerTwo
        {
            get { return _wrongAnswerTwo; }
            set { OnPropertyChanged(ref _wrongAnswerTwo, value); }
        }
        public string WrongAnswerThree
        {
            get { return _wrongAnswerThree; }
            set { OnPropertyChanged(ref _wrongAnswerThree, value); }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public int QuestionDisplayTime
        {
            get { return _questionDisplayTime; }
            set { _questionDisplayTime = value; }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public DispatcherTimer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }
        //Validatie
        public event PropertyChangedEventHandler PropertyChanged;
        [System.Text.Json.Serialization.JsonIgnore]
        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "TheQuestion")
                {
                    if (string.IsNullOrEmpty(TheQuestion))
                    {
                        result = "Geef een vraag in.";
                    }
                }
                if (columnName == "CorrectAnswer")
                {
                    if (string.IsNullOrEmpty(CorrectAnswer))
                    {
                        result = "Geef een correct antwoord in.";
                    }
                }
                if (columnName == "WrongAnswerOne")
                {
                    if (string.IsNullOrEmpty(WrongAnswerOne))
                    {
                        result = "Gelieve een fout antwoord in te voeren.";
                    }
                }
                if (columnName == "WrongAnswerTwo")
                {
                    if (string.IsNullOrEmpty(WrongAnswerTwo))
                    {
                        result = "Gelieve een fout antwoord in te voeren.";
                    }
                }
                if (columnName == "WrongAnswerThree")
                {
                    if (string.IsNullOrEmpty(WrongAnswerThree))
                    {
                        result = "Gelieve een fout antwoord in te voeren.";
                    }
                }
                return result;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool OnPropertyChanged<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            bool isChanged = true;
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                isChanged = false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return isChanged;
        }

        //Methodes Timer
        public void StartTimer() //Settings van timer instellen dan -> start de timer - Dit kon mss in constructor (?)
        {
            Timer.Start();
        }
        public void Timer_Tick(object sender, EventArgs e) //Elke tick (1 tick = 1 seconde) , de displaytime met 1 naar beneden, als de displaytime 0 is, stop de timer
        {
            QuestionDisplayTime--;
            if (QuestionDisplayTime == 0)
            {
                Timer.Stop();
            }
        }
        //Methodes Answers/Buttons
        //Antwoorden in random buttons steken
        public void InsertAnswersInButtons(Button topLeftButton1, Button topRightButton2, Button bottomLeftButton3, Button bottomRightButton4)
        {
            Random random = new Random();
            switch (random.Next(0, 5)) //Rollt values 0,1,2,3 en 4
            {
                case 0:
                    topLeftButton1.Content = CorrectAnswer;
                    topRightButton2.Content = WrongAnswerOne;
                    bottomLeftButton3.Content = WrongAnswerTwo;
                    bottomRightButton4.Content = WrongAnswerThree;
                    break;
                case 1:
                    topLeftButton1.Content = WrongAnswerTwo;
                    topRightButton2.Content = CorrectAnswer;
                    bottomLeftButton3.Content = WrongAnswerThree;
                    bottomRightButton4.Content = WrongAnswerOne;
                    break;
                case 2:
                    topLeftButton1.Content = WrongAnswerOne;
                    topRightButton2.Content = WrongAnswerThree;
                    bottomLeftButton3.Content = CorrectAnswer;
                    bottomRightButton4.Content = WrongAnswerTwo;
                    break;
                case 3:
                    topLeftButton1.Content = WrongAnswerThree;
                    topRightButton2.Content = WrongAnswerTwo;
                    bottomLeftButton3.Content = WrongAnswerOne;
                    bottomRightButton4.Content = CorrectAnswer;
                    break;
                case 4:
                    topLeftButton1.Content = CorrectAnswer; ;
                    topRightButton2.Content = WrongAnswerTwo;
                    bottomLeftButton3.Content = WrongAnswerOne;
                    bottomRightButton4.Content = WrongAnswerThree;
                    break;
            }
        }
        //Highlight if wrong or correct answer
        public void AnswerHighlight(Button selectedButton)
        {
            if ((string)selectedButton.Content == CorrectAnswer)
            {
                selectedButton.Background = Brushes.Green;
            }
            else
            {
                selectedButton.Background = Brushes.OrangeRed;
            }
        }
        //Looped door de buttons en laat het correcte antwoord zien (gebruiken wanneer niemand het juist heeft)
        public void ShowCorrectAnswer(List<Button> lijstButtons)
        {
            foreach (Button button in lijstButtons)
            {
                if ((string)button.Content == CorrectAnswer)
                {
                    button.Background = Brushes.Green;
                }
            }
        }
        public override string ToString()
        {
            return $"{TheQuestion}{Environment.NewLine}{CorrectAnswer}{Environment.NewLine}{WrongAnswerOne}{Environment.NewLine}{WrongAnswerTwo}{Environment.NewLine}{WrongAnswerThree}";
        }
    }
}

