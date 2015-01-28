﻿using System;
using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;

namespace Wiktionary.ViewModel
{
    public class ParametrerViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour
        private string _username;
        public string Username //username à modifier
        {
            get
            {
                return _username;
            }

            set
            {
                if (_username != value)
                {
                    _username = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ParametrerViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            //Bouton Modifier
            Modifier = new RelayCommand(ModifierUsername);
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Ajoute ou modifie le username
        private async void ModifierUsername()
        {
            MessageDialog msgDialog = new MessageDialog("Le username " + _username + " a été modifié avec succès!", "Félicitation");
            await msgDialog.ShowAsync();
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        
    }
}
