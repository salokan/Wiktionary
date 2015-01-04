﻿using System.ComponentModel;

namespace Wiktionary.Models
{
    public class Definitions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _mot;
        public string Mot
        {
            get
            {
                return _mot;
            }

            set
            {
                if (_mot != value)
                {
                    _mot = value;
                    OnPropertyChanged("Mot");
                }
            }
        }

        private string _definition;
        public string Definition
        {
            get
            {
                return _definition;
            }

            set
            {
                if (_definition != value)
                {
                    _definition = value;
                    OnPropertyChanged("Definition");
                }
            }

        }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
