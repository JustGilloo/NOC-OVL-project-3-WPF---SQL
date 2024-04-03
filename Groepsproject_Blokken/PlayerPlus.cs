using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
//Todo nog niks :)
namespace Groepsproject_Blokken
{
    public partial class Player : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _nameValidation;
        private string _passwordValidation;
        private string _passwordConfirmValidation;
        private BitmapImage _bmp;

        [System.Text.Json.Serialization.JsonIgnore]
        public BitmapImage BMP
        {
            get { return _bmp; }
            set { _bmp = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [System.Text.Json.Serialization.JsonIgnore]

        public string NameValidation
        {
            get { return _nameValidation; }
            set
            {
                OnPropertyChanged(ref _nameValidation, value);
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]

        public string PasswordValidation
        {
            get { return _passwordValidation; }
            set
            {
                OnPropertyChanged(ref _passwordValidation, value);
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]

        public string PasswordConfirmValidation
        {
            get { return _passwordConfirmValidation; }
            set
            {
                OnPropertyChanged(ref _passwordConfirmValidation, value);
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "NameValidation")
                {
                    if (string.IsNullOrEmpty(NameValidation))
                    {
                        result = "Geef een naam in.";
                    }
                }
                if (columnName == "PasswordValidation")
                {
                    if (string.IsNullOrEmpty(PasswordValidation))
                    {
                        result = "Geef een paswoord in.";
                    }
                }
                if (columnName == "PasswordConfirmValidation")
                {
                    if (string.IsNullOrEmpty(PasswordConfirmValidation))
                    {
                        result = "Gelieve uw paswoord te bevestigen.";
                    }
                }
                return result;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Error => throw new System.Exception();

        [System.Text.Json.Serialization.JsonIgnore]
        public int Position { get; internal set; }

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

        public string CalculateSPWinRate() //Zelfde logica dan bij functie CalculateWinRates();
        {
            double winrate = 0.00;
            if (SPGamesWon != null)
            {
                winrate = (double)((double)SPGamesWon / SPGamesPlayed * 100);
            }
            string winrateSP = winrate.ToString("0.00") + "%";
            if (this.SPGamesPlayed == null)
            {
                winrateSP = "No games played yet!";
            }
            return winrateSP;
        }
        public string CalculateVSWinRate()
        {
            double winrate = 0.00;
            if (VSGamesWon != null)
            {
                winrate = (double)((double)VSGamesWon / VSGamesPlayed * 100);
            }
            string winrateVS = winrate.ToString("0.00") + "%";
            if (this.VSGamesPlayed == null)
            {
                winrateVS = "No games played yet!";
            }
            return winrateVS;
        }
        public override bool Equals(object obj)
        {
            bool resultaat = false;
            if (obj != null)
            {
                if (GetType() == obj.GetType())
                {
                    Player g = (Player)obj;
                    if (this.Name == g.Name)
                    {
                        resultaat = true;
                    }
                }
            }
            return resultaat;
        }
        public override string ToString()
        {
            return this.Name;
        }
        public void ImageInladenMetMemoryStream() // De BMP property inladen met memorystream
        {
            BMP = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            FileStream stream = new FileStream(this.ProfilePicture, FileMode.Open, FileAccess.Read);
            ms.SetLength(stream.Length);
            stream.Read(ms.GetBuffer(), 0, (int)stream.Length);
            ms.Flush();
            stream.Close();
            BMP.BeginInit();
            BMP.StreamSource = ms;
            BMP.EndInit();
        }

    }
}

