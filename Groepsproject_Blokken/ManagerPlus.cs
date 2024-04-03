using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Groepsproject_Blokken
{
    public partial class Manager : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _nameValidation;
        private string _passwordValidation;
        private string _passwordConfirmValidation;

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
        public override bool Equals(object obj)
        {
            bool resultaat = false;
            if (obj != null)
            {
                if (GetType() == obj.GetType())
                {
                    Manager g = (Manager)obj;
                    if (this.Name == g.Name)
                    {
                        resultaat = true;
                    }
                }
            }
            return resultaat;
        }

    }
}
