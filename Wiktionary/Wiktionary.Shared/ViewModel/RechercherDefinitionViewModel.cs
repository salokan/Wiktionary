using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;
using Wiktionary.Models;

namespace Wiktionary.ViewModel
{
    public class RechercherDefinitionViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<Definitions> DefinitionsRecherchees { get; set; } //Liste des définitions recherchées
        private ObservableCollection<Definitions> definitionsRecherchees = new ObservableCollection<Definitions>();
        private ObservableCollection<Definitions> toutesDefinitions = new ObservableCollection<Definitions>(); //Liste contenant toutes les définitions
        public ICommand Rechercher { get; set; } //Bouton Rechercher
        public ICommand Supprimer { get; set; } //Bouton Supprimer
        public ICommand Retour { get; set; } //Bouton Retour

        private string motRecherche;
        public string MotRecherche//TextBox du mot dont on veut trouver la définition
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
        public Definitions MotSelectionne//Valeur du mot sélectionné
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

            Rechercher = new RelayCommand(RechercherDefinition);
            Supprimer = new RelayCommand(SupprimerDefinition);
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Permet de récupérer toutes les définitions et de les insérer dans une même liste
        private void initDefinitionListe()
        {
            ObservableCollection<Definitions> definitionsLocales = new ObservableCollection<Definitions>();
            ObservableCollection<Definitions> definitionsRoaming = new ObservableCollection<Definitions>();
            ObservableCollection<Definitions> definitionsPubliques = new ObservableCollection<Definitions>();

            //Définitions locales
            definitionsLocales.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa" });
            definitionsLocales.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb" });
            definitionsLocales.Add(new Definitions { Mot = "c", Definition = "cccccccccccc" });
            definitionsLocales.Add(new Definitions { Mot = "a", Definition = "dddddddddddd" });
            definitionsLocales.Add(new Definitions { Mot = "a", Definition = "eeeeeeeeeeee" });
            definitionsLocales.Add(new Definitions { Mot = "a", Definition = "ffffffffffff" });
            definitionsLocales.Add(new Definitions { Mot = "a", Definition = "gggggggggggg" });

            //Définitions roaming
            definitionsRoaming.Add(new Definitions { Mot = "d", Definition = "dddddddddddd" });
            definitionsRoaming.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee" });
            definitionsRoaming.Add(new Definitions { Mot = "f", Definition = "ffffffffffff" });
            definitionsRoaming.Add(new Definitions { Mot = "g", Definition = "gggggggggggg" });

            //Définitions publiques
            definitionsPubliques.Add(new Definitions { Mot = "a", Definition = "hhhhhhhhhhhh" });
            definitionsPubliques.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh" });
            definitionsPubliques.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii" });
            definitionsPubliques.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj" });

            //Toutes définitions
            foreach (Definitions dLocales in definitionsLocales)
            {
                toutesDefinitions.Add(dLocales);
            }
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

        //Supprimer la définition sélectionnée
        private void SupprimerDefinition()
        {
            Definitions definitionASupprimer = new Definitions();
            bool aSupprimer = false;
            if (motSelectionne != null)
            {
                foreach (Definitions d in toutesDefinitions)
                    if (d.Mot.Equals(motSelectionne.Mot) && d.Definition.Equals(motSelectionne.Definition))
                    {
                        definitionASupprimer = d;
                        aSupprimer = true;
                    }

                if (aSupprimer)
                {
                    toutesDefinitions.Remove(definitionASupprimer);
                    definitionsRecherchees.Remove(definitionASupprimer);
                }
                
                DefinitionsRecherchees = definitionsRecherchees;
            }
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }
    }
}
