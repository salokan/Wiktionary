using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Data.Json;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLite;
using Wiktionary.Controllers;
using Wiktionary.Models;
using Wiktionary.Views;

namespace Wiktionary.ViewModel
{
    public class ListeDefinitionsViewModel : ViewModelBase, IViewModel
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private readonly INavigationService _navigationService;
        public ObservableCollection<Definitions> Definitions { get; set; } //Liste des définitions à afficher
        private ObservableCollection<Definitions> definitions = new ObservableCollection<Definitions>();
        public ICommand Toutes { get; set; } //Bouton Toutes
        public ICommand Locales { get; set; } //Bouton Locales
        public ICommand Roaming { get; set; } //Bouton Roaming
        public ICommand Publiques { get; set; } //Bouton Publiques
        public ICommand Supprimer { get; set; } //Bouton Supprimer
        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour

        private string _typeDefinitions;
        public string TypeDefinitions//TextBlock des définitions affichés
        {
            get
            {
                return _typeDefinitions;
            }

            set
            {
                if (_typeDefinitions != value)
                {
                    _typeDefinitions = value;
                    RaisePropertyChanged();
                    //RaisePropertyChanged("TypeDefinitions");
                }
            }
        }

        private Definitions _motSelectionne;
        public Definitions MotSelectionne //Valeur du mot sélectionné dans la liste
        {
            get
            {
                return _motSelectionne;
            }

            set
            {
                if (_motSelectionne != value)
                {
                    _motSelectionne = value;
                    RaisePropertyChanged();
                }
            }
        }

        //Constructeur
        public ListeDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Toutes = new RelayCommand(ToutesDefinitions); //Bouton Toutes
            Locales = new RelayCommand(DefinitionsLocales); //Bouton Locales
            Roaming = new RelayCommand(DefinitionsRoaming); //Bouton Roaming
            Publiques = new RelayCommand(DefinitionsPubliques); //Bouton Publiques
            Supprimer = new RelayCommand(SupprimerDefinition); //Bouton Supprimer
            Modifier = new RelayCommand(AfficherModifierDefinition); //Bouton Modifier
            Retour = new RelayCommand(AfficherPagePrecedente); //Bouton Retour
        }

        //Initialise la liste des définition
        private void InitListe()
        {
            TypeDefinitions = "Toutes";

            definitions.Clear();

            GetDefinitionsLocales(); //Insère dans la liste les définitions de la base de données
            GetDefinitionsRoaming(); //Insère dans la liste les définitions roaming
            GetDefinitionsPubliques(); //Insère dans la liste les définitions du webservice

            Definitions = definitions;
        }

        #region Afficher les définitions
        //Afficher toutes les définitions
        private void ToutesDefinitions()
        {
            TypeDefinitions = "Toutes";

            definitions.Clear();

            GetDefinitionsLocales(); //Insère dans la liste les définitions de la base de données
            GetDefinitionsRoaming(); //Insère dans la liste les définitions roaming
            GetDefinitionsPubliques(); //Insère dans la liste les définitions du webservice

            Definitions = definitions;
        }

        //Afficher toutes les définitions locales
        private void DefinitionsLocales()
        {
            TypeDefinitions = "Locales";

            definitions.Clear();

            GetDefinitionsLocales();  //Insère dans la liste les définitions de la base de données

            Definitions = definitions;
        }

        //Afficher toutes les définitions roaming
        private void DefinitionsRoaming()
        {
            TypeDefinitions = "Roaming";

            definitions.Clear();

            GetDefinitionsRoaming(); //Insère dans la liste les définitions roaming

            Definitions = definitions;
        }

        //Afficher toutes les définitions publiques
        private void DefinitionsPubliques()
        {
            TypeDefinitions = "Publiques";

            definitions.Clear();

            GetDefinitionsPubliques(); //Insère dans la liste les définitions du webservice

            Definitions = definitions;
        }
        #endregion

        #region Récupérer les définitons
        //Permet de récupérer toutes les définitions dans la base de données
        private async void GetDefinitionsLocales()
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            var result = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions");
            foreach (var item in result)
            {
                definitions.Add(new Definitions { Mot = item.Mot, Definition = item.Definition, TypeDefinition = "locale" });
            }
        }

        //Permet de récupérer toutes les définitions roaming
        private async void GetDefinitionsRoaming()
        {
            await RoamingStorage.Restore<Definitions>();

            foreach (var item in RoamingStorage.Data)
            {
                definitions.Add(item as Definitions);
            }
        }

        //Permet de récupérer toutes les définitions via le webservice
        private async void GetDefinitionsPubliques()
        {
            Webservices ws = new Webservices();

            string response = await ws.GetAllDefinitions();

            string definitionsJson = "{\"definitions\":" + response + "}";

            JsonObject jsonObject = JsonObject.Parse(definitionsJson);

            List<DefinitionsPubliques> definitionsPubliques = new List<DefinitionsPubliques>();

            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray("definitions"))
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    definitionsPubliques.Add(new DefinitionsPubliques(jsonValue.GetObject()));
                }
            }

            foreach (DefinitionsPubliques dp in definitionsPubliques)
            {
                definitions.Add(new Definitions { Mot = dp.Word, Definition = dp.Definition, TypeDefinition = "publique" });
            }
        }
        #endregion

        #region Supprimer les définitions
        //Supprimer la définition sélectionnée
        private async void SupprimerDefinition()
        {
            if (_motSelectionne != null)
            {
                MessageDialog msgDialog =
                    new MessageDialog("Etes-vous sûr de vouloir supprimer la définition sélectionnée?",
                        "Demande de confirmation");
                msgDialog.Commands.Add(new UICommand("Oui", command =>
                                                            {
                                                                Definitions definitionASupprimer = new Definitions();
                                                                bool aSupprimer = false;

                                                                foreach (Definitions d in definitions)
                                                                    if (d.Mot.Equals(_motSelectionne.Mot) &&
                                                                        d.Definition.Equals(_motSelectionne.Definition) &&
                                                                        d.TypeDefinition.Equals(
                                                                            _motSelectionne.TypeDefinition))
                                                                    {
                                                                        definitionASupprimer = d;
                                                                        aSupprimer = true;
                                                                    }

                                                                if (aSupprimer && !definitionASupprimer.TypeDefinition.Equals("publique"))
                                                                {
                                                                    definitions.Remove(definitionASupprimer);
                                                                }

                                                                if (definitionASupprimer.TypeDefinition.Equals("locale"))
                                                                {
                                                                    SupprimerLocale(definitionASupprimer);
                                                                }
                                                                else if (
                                                                    definitionASupprimer.TypeDefinition.Equals(
                                                                        "roaming"))
                                                                {
                                                                    SupprimerRoaming(definitionASupprimer);
                                                                }
                                                                else if (definitionASupprimer.TypeDefinition.Equals(
                                                                    "publique"))
                                                                {
                                                                    SupprimerPublique(definitionASupprimer);
                                                                }

                                                                Definitions = definitions;
                                                            }));
                msgDialog.Commands.Add(new UICommand("Non", command =>
                                                           {

                                                           }));
                await msgDialog.ShowAsync();
            }
        }

        //Supprimer de la base la Définition en paramètre
        private async void SupprimerLocale(Definitions def)
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            var definitionDelete = await connection.Table<DefinitionsTable>().Where(x => x.Mot.Equals(def.Mot)).FirstOrDefaultAsync();

            if (definitionDelete != null)
            {
                await connection.DeleteAsync(definitionDelete);
            }

            MessageDialog msgDialog = new MessageDialog("Le mot " + def.Mot + " : " + def.Definition + " a été supprimé avec succès en locale!", "Félicitation");
            await msgDialog.ShowAsync();
        }

        private async void SupprimerRoaming(Definitions def)
        {
            await RoamingStorage.Restore<Definitions>();

            Definitions d = new Definitions();
            string mot = def.Mot;

            foreach (var item in RoamingStorage.Data)
            {
                Definitions defRoaming = item as Definitions;
                if (defRoaming != null && defRoaming.Mot.Equals(mot))
                    d = item as Definitions;

            }

            RoamingStorage.Data.Remove(d);

            await RoamingStorage.Save<Definitions>();
        }

        //Supprime la définition avec les webservices
        private async void SupprimerPublique(Definitions def)
        {
            Webservices ws = new Webservices();
            string response = await ws.DeleteDefinition(def.Mot, localSettings.Values["Username"].ToString());

            if (response.Equals("\"Success\""))
            {
                definitions.Remove(def);

                Definitions = definitions;

                MessageDialog msgDialog = new MessageDialog("Le mot " + def.Mot + " : " + def.Definition + " a été supprimé avec succès en publique!", "Félicitation");
                await msgDialog.ShowAsync();
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Vous n'avez pas ajouter le mot " + def.Mot + " : " + def.Definition + " donc vous ne pouvez pas le supprimer!", "Attention");
                await msgDialog.ShowAsync();
            } 
        }
        #endregion

        //Naviguer sur la page Modifier
        private void AfficherModifierDefinition()
        {
            if (_motSelectionne != null)
            {
                _navigationService.Navigate(typeof(ModifierDefinitions), _motSelectionne);
            }

        } 

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        //Récupère le paramètre contenant la définition à modifier
        public void GetParameter(object parameter)
        {
            
        }

        //Permet de réinitialiser la liste à chaque fois que l'on navigue sur cette page
        public void OnNavigatedTo()
        {
            InitListe();
        }
    }
}