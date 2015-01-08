using System.Collections.ObjectModel;
using System.Windows.Input;
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

        ObservableCollection<Definitions> definitionsLocales = new ObservableCollection<Definitions>();
        ObservableCollection<Definitions> definitionsRoaming = new ObservableCollection<Definitions>();
        ObservableCollection<Definitions> definitionsPubliques = new ObservableCollection<Definitions>();

        private string motRecherche;
        public string MotRecherche //TextBox du mot dont on veut trouver la définition
        {
            get
            {
                return motRecherche;
            }

            set
            {
                if (motRecherche != value)
                {
                    motRecherche = value;
                    RaisePropertyChanged("MotRecherche");
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
        public RechercherDefinitionViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            initDefinitionListe();

            DefinitionsRecherchees = definitionsRecherchees;

            //Bouton Rechercher
            Rechercher = new RelayCommand(RechercherDefinition);
            //Bouton Modifier
            Modifier = new RelayCommand(AfficherModifierDefinition);
            //Bouton Supprimer
            Supprimer = new RelayCommand(SupprimerDefinition);
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Permet de récupérer toutes les définitions et de les insérer dans une même liste
        private void initDefinitionListe()
        {
            toutesDefinitions.Clear();

            //Définitions locales
            GetDefinitionsLocales();

            //Définitions roaming
            definitionsRoaming.Add(new Definitions { Mot = "d", Definition = "dddddddddddd", TypeDefinition = "roaming" });
            definitionsRoaming.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee", TypeDefinition = "roaming" });
            definitionsRoaming.Add(new Definitions { Mot = "f", Definition = "ffffffffffff", TypeDefinition = "roaming" });
            definitionsRoaming.Add(new Definitions { Mot = "g", Definition = "gggggggggggg", TypeDefinition = "roaming" });

            //Définitions publiques
            definitionsPubliques.Add(new Definitions { Mot = "a", Definition = "hhhhhhhhhhhh", TypeDefinition = "publique" });
            definitionsPubliques.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa", TypeDefinition = "publique" });
            definitionsPubliques.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh", TypeDefinition = "publique" });
            definitionsPubliques.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii", TypeDefinition = "publique" });
            definitionsPubliques.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj", TypeDefinition = "publique" });

            //Toutes définitions
            foreach (Definitions dRoaming in definitionsRoaming)
            {
                toutesDefinitions.Add(dRoaming);
            }
            foreach (Definitions dPubliques in definitionsPubliques)
            {
                toutesDefinitions.Add(dPubliques);
            }
        }

        //Rechercher une définition
        private void RechercherDefinition()
        {
            definitionsRecherchees.Clear();
            foreach (Definitions d in toutesDefinitions)
            {
                if (d.Mot.Equals(motRecherche))
                    definitionsRecherchees.Add(d);
            }

            DefinitionsRecherchees = definitionsRecherchees;
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

                                                                foreach (Definitions d in toutesDefinitions)
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
                msgDialog.Commands.Add(new UICommand("No", (command) =>
                {

                }));
                msgDialog.ShowAsync();
            }
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            initDefinitionListe();

            _navigationService.GoBack();
        }

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

        private void SupprimerRoaming(Definitions def)
        {
            MessageDialog msgDialog = new MessageDialog("Le mot " + def.Mot + " : " + def.Definition + " a été supprimé avec succès en roaming!", "Félicitation");
            msgDialog.ShowAsync();
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
            initDefinitionListe();
        }
    }
}
