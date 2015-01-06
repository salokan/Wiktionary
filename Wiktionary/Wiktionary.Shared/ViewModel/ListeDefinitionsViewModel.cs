using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;
using Wiktionary.Models;

namespace Wiktionary.ViewModel
{
    public class ListeDefinitionsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public ObservableCollection<Definitions> Definitions { get; set; } //Liste des définitions à afficher
        private ObservableCollection<Definitions> definitions = new ObservableCollection<Definitions>();
        public ICommand Toutes { get; set; } //Bouton Toutes
        public ICommand Locales { get; set; } //Bouton Locales
        public ICommand Roaming { get; set; } //Bouton Roaming
        public ICommand Publiques { get; set; } //Bouton Publiques
        public ICommand Supprimer { get; set; } //Bouton Supprimer
        public ICommand Retour { get; set; } //Bouton Retour

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

            TypeDefinitions = "Toutes";
            
            //Liste des définitions à afficher
            definitions.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "c", Definition = "cccccccccccc", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii", TypeDefinition = "publique" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj", TypeDefinition = "publique" });

            Definitions = definitions;

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
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }



        //Afficher toutes les définitions
        private void ToutesDefinitions()
        {
            TypeDefinitions = "Toutes";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "c", Definition = "cccccccccccc", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg", TypeDefinition = "roaming" });
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

            definitions.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb", TypeDefinition = "locale" });
            definitions.Add(new Definitions { Mot = "c", Definition = "cccccccccccc", TypeDefinition = "locale" });

            Definitions = definitions;
        }

        //Afficher toutes les définitions roaming
        private void DefinitionsRoaming()
        {
            TypeDefinitions = "Roaming";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff", TypeDefinition = "roaming" });
            definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg", TypeDefinition = "roaming" });

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
            _navigationService.GoBack();
        }

        
    }
}
