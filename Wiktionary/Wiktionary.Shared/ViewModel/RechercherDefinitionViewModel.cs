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
    public class RechercherDefinitionViewModel : ViewModelBase, IViewModel
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<Definitions> DefinitionsRecherchees { get; set; } //Liste des définitions recherchées
        private ObservableCollection<Definitions> definitionsRecherchees = new ObservableCollection<Definitions>();
        private ObservableCollection<Definitions> toutesDefinitions = new ObservableCollection<Definitions>(); //Liste contenant toutes les définitions
        public ICommand Rechercher { get; set; } //Bouton Rechercher
        public ICommand Supprimer { get; set; } //Bouton Supprimer
        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour

        private string _motRecherche;
        public string MotRecherche //TextBox du mot dont on veut trouver la définition
        {
            get
            {
                return _motRecherche;
            }

            set
            {
                if (_motRecherche != value)
                {
                    _motRecherche = value;
                    RaisePropertyChanged();
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
        public RechercherDefinitionViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            DefinitionsRecherchees = definitionsRecherchees;

            Rechercher = new RelayCommand(RechercherDefinition); //Bouton Rechercher
            Modifier = new RelayCommand(AfficherModifierDefinition); //Bouton Modifier
            Supprimer = new RelayCommand(SupprimerDefinition); //Bouton Supprimer
            Retour = new RelayCommand(AfficherPagePrecedente); //Bouton Retour
        }

        //Permet de récupérer toutes les définitions et de les insérer dans une même liste
        private void InitListe()
        {
            definitionsRecherchees.Clear();
            MotRecherche = "";
            toutesDefinitions.Clear();

            GetDefinitionsLocales(); //Définitions locales
            GetDefinitionsRoaming(); //Définitions roaming
            GetDefinitionsPubliques(); //Définitions publiques
        }

        #region Récupérer les définitons
        //Permet de récupérer toutes les définitions dans la base de données
        private async void GetDefinitionsLocales()
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            var result = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions");
            foreach (var item in result)
            {
                toutesDefinitions.Add(new Definitions { Mot = item.Mot, Definition = item.Definition, TypeDefinition = "locale" });
            }
        }

        //Permet de récupérer toutes les définitions roaming
        private async void GetDefinitionsRoaming()
        {
            await RoamingStorage.Restore<Definitions>();

            foreach (var item in RoamingStorage.Data)
            {
                toutesDefinitions.Add(item as Definitions);
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
                toutesDefinitions.Add(new Definitions { Mot = dp.Word, Definition = dp.Definition, TypeDefinition = "publique" });
            }
        }
        #endregion

        #region Rechercher une définition
        //Rechercher une définition
        private void RechercherDefinition()
        {
            definitionsRecherchees.Clear();
            foreach (Definitions d in toutesDefinitions)
            {
                if (d.Mot.Equals(_motRecherche))
                    definitionsRecherchees.Add(d);
            }

            DefinitionsRecherchees = definitionsRecherchees;
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

                                                                foreach (Definitions d in toutesDefinitions)
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
                                                                    toutesDefinitions.Remove(definitionASupprimer);
                                                                    definitionsRecherchees.Remove(definitionASupprimer);
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

                                                                DefinitionsRecherchees = definitionsRecherchees;
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

        //Supprime la définition via le web service
        private async void SupprimerPublique(Definitions def)
        {
            Webservices ws = new Webservices();
            string response = await ws.DeleteDefinition(def.Mot, "gregnico");

            if (response.Equals("\"Success\""))
            {
                definitionsRecherchees.Remove(def);
                toutesDefinitions.Remove(def);

                DefinitionsRecherchees = definitionsRecherchees;

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

        //Permet de réinitialiser la liste si on arrive depuis un retour
        public void OnNavigatedTo()
        {
            InitListe();
        }
    }
}