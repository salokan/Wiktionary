using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
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

        public bool isBack = false;

        private string typeDefinitions;
        public string TypeDefinitions//TextBlock des définitions affichés
        {
            get
            {
                return typeDefinitions;
            }

            set
            {
                if (typeDefinitions != value)
                {
                    typeDefinitions = value;
                    RaisePropertyChanged("TypeDefinitions");
                }
            }
        }

        private Definitions motSelectionne;
        public Definitions MotSelectionne //Valeur du mot sélectionné dans la liste
        {
            get
            {
                return motSelectionne;
            }

            set
            {
                if (motSelectionne != value)
                {
                    motSelectionne = value;
                    RaisePropertyChanged("MotSelectionne");
                }
            }
        }

        //Constructeur
        public ListeDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            initListe();

            //Bouton Toutes
            Toutes = new RelayCommand(ToutesDefinitions);
            //Bouton Locales
            Locales = new RelayCommand(DefinitionsLocales);
            //Bouton Roaming
            Roaming = new RelayCommand(DefinitionsRoaming);
            //Bouton Publiques
            Publiques = new RelayCommand(DefinitionsPubliques);
            //Bouton Supprimer
            Supprimer = new RelayCommand(SupprimerDefinition);
            //Bouton Modifier
            Modifier = new RelayCommand(AfficherModifierDefinition);
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Initialise la liste des définition
        private async void initListe()
        {
            TypeDefinitions = "Toutes";

            definitions.Clear();

            GetDefinitionsLocales(); //Insère dans la liste les définitions de la base de données
            //Liste des définitions à afficher

            //await RoamingStorage.Restore<Definitions>();

            //SetDefList();

            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj", TypeDefinition = "publique" });

            Definitions = definitions;
        }

        //Afficher toutes les définitions
        private void ToutesDefinitions()
        {
            TypeDefinitions = "Toutes";

            definitions.Clear();

            GetDefinitionsLocales();
           
            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj", TypeDefinition = "publique" });

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
        private async void DefinitionsRoaming()
        {
            TypeDefinitions = "Roaming";

          //  definitions.Add(new Definitions { Mot = (List<Definitions>)roamingSettings.Values["test"], Definition = def.Definition, TypeDefinition = "roaming" });

            //definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd", TypeDefinition = "roaming" });
            //definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee", TypeDefinition = "roaming" });
            //definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff", TypeDefinition = "roaming" });
            //definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg", TypeDefinition = "roaming" });

            

           await RoamingStorage.Restore<Definitions>();

           SetDefList();
          


        }

        private void SetDefList()
        {
            foreach (var item in RoamingStorage.Data)
            {
                definitions.Add(item as Definitions);
            }
            Definitions = definitions;
        }

  

        //Afficher toutes les définitions publiques
        private void DefinitionsPubliques()
        {
            TypeDefinitions = "Publiques";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj", TypeDefinition = "publique" });

            Definitions = definitions;
        }

        //Naviguer sur la page Modifier
        private void AfficherModifierDefinition()
        {
            if (motSelectionne != null)
            {
                _navigationService.Navigate(typeof(ModifierDefinitions), motSelectionne);
            }
            
        } 

        //Supprimer la définition sélectionnée
        private void SupprimerDefinition()
        {
            if (motSelectionne != null)
            {
                MessageDialog msgDialog =
                    new MessageDialog("Etes-vous sûr de vouloir supprimer la définition sélectionnée?",
                        "Demande de confirmation");
                msgDialog.Commands.Add(new UICommand("Yes", (command) =>
                                                            {
                                                                Definitions definitionASupprimer = new Definitions();
                                                                bool aSupprimer = false;

                                                                foreach (Definitions d in definitions)
                                                                    if (d.Mot.Equals(motSelectionne.Mot) &&
                                                                        d.Definition.Equals(motSelectionne.Definition) &&
                                                                        d.TypeDefinition.Equals(
                                                                            motSelectionne.TypeDefinition))
                                                                    {
                                                                        definitionASupprimer = d;
                                                                        aSupprimer = true;
                                                                    }

                                                                if (aSupprimer)
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
                msgDialog.Commands.Add(new UICommand("No", (command) =>
                                                           {

                                                           }));
                msgDialog.ShowAsync();
            }
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            initListe();

            _navigationService.GoBack();
        }

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

        //Supprimer de la base la Définition en paramètre
        private async void SupprimerLocale(Definitions def)
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            var DefinitionDelete = await connection.Table<DefinitionsTable>().Where(x => x.Mot.Equals(def.Mot)).FirstOrDefaultAsync();

            if (DefinitionDelete != null)
            {
                await connection.DeleteAsync(DefinitionDelete);
            }

            MessageDialog msgDialog = new MessageDialog("Le mot " + def.Mot + " : " + def.Definition + " a été supprimé avec succès en locale!", "Félicitation");
            msgDialog.ShowAsync();
        }

        private async void SupprimerRoaming(Definitions def)
        {
            await RoamingStorage.Restore<Definitions>();
            DelDefList(def.Mot);

            RoamingStorage.Save<Definitions>();
            //MessageDialog msgDialog = new MessageDialog("Le mot " + def.Mot + " : " + def.Definition + " a été supprimé avec succès en roaming!", "Félicitation");
            //msgDialog.ShowAsync();
        }

        private void DelDefList(string mot)
        {
            Definitions d = new Definitions();
            
            foreach (var item in RoamingStorage.Data)
            {
                Definitions def = item as Definitions;
                if (def.Mot.Equals(mot))
                    d = item as Definitions;

            }

                RoamingStorage.Data.Remove(d);
                Definitions = definitions;
        }

        private void SupprimerPublique(Definitions def)
        {
            MessageDialog msgDialog = new MessageDialog("Le mot " + def.Mot + " : " + def.Definition + " a été supprimé avec succès en public!", "Félicitation");
            msgDialog.ShowAsync();
        }

        //Récupère le paramètre contenant la définition à modifier
        public void GetParameter(object parameter)
        {
            
        }

        //Permet de réinitialiser la liste si on arrive depuis un retour
        public void GetIsBack()
        {
            initListe();
        }
    }
}
