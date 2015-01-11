﻿using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLite;
using Wiktionary.Controllers;
using Wiktionary.Models;

namespace Wiktionary.ViewModel
{
    public class AjouterDefinitionsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand Locale { get; set; } //Bouton Ajouter Locale
        public ICommand Roaming { get; set; } //Bouton Ajouter Roaming
        public ICommand Publique { get; set; } //Bouton Ajouter Publique
        public ICommand Retour { get; set; } //Bouton Retour

        private string _mot;
        public string Mot //Mot à ajouter
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
                    RaisePropertyChanged();
                }
            }
        }

        private string _definition;
        public string Definition //Définition à ajouter
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
                    RaisePropertyChanged();
                }
            }
        }

        //Constructeur
        public AjouterDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            //Bouton Locale
            Locale = new RelayCommand(AjouterLocale);

            //Bouton Roaming
            Roaming = new RelayCommand(AjouterRoaming);

            //Bouton Publique
            Publique = new RelayCommand(AjouterPublique);

            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Ajouter un définition locale
        private async void AjouterLocale()
        {
            bool existeDeja = false;
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            var result = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions WHERE Mot = ?", new object[] { _mot });
            foreach (var item in result)
            {
                if (item.Mot.Equals(_mot))
                    existeDeja = true;
            }

            if (existeDeja)
            {
                MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " possède déjà une définition en locale", "Attention");
                msgDialog.ShowAsync();
            }
            else
            {
                var definitionInsert = new DefinitionsTable
                {
                    Mot = _mot,
                    Definition = _definition
                };
                await connection.InsertAsync(definitionInsert);

                MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " : " + _definition + " a été ajouté avec succès en local!", "Félicitation");
                msgDialog.ShowAsync();
            }
            
        }

        //Ajouter un définition roaming
        private void AjouterRoaming()
        {
            MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " : " + _definition + " a été ajouté avec succès en roaming!", "Félicitation");
            msgDialog.ShowAsync();
        }

        //Ajouter un définition publique
        private async void AjouterPublique()
        {
            Webservices ws = new Webservices();
            string response = await ws.AddDefinition(_mot,_definition, "gregnico");

            if (response.Equals("\"Success\""))
            {
                MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " : " + _definition + " a été ajouté avec succès en publique!", "Félicitation");
                msgDialog.ShowAsync();
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " possède déjà une définition en publique", "Attention");
                msgDialog.ShowAsync();
            }   
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
           _navigationService.GoBack();
            

            //string response = await ws.GetDefinition("coucou");

            //JsonObject jsonObject = JsonObject.Parse(response);

            //string mot = jsonObject.GetNamedString("Word");
            //string definition = jsonObject.GetNamedString("Definition");
        }
    }
}