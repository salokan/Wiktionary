using System.Collections.ObjectModel;
using System.Windows.Input;
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

        //Constructeur
        public ListeDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            TypeDefinitions = "Toutes";
            
            //Liste des définitions à afficher
            definitions.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa" });
            definitions.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb" });
            definitions.Add(new Definitions { Mot = "c", Definition = "cccccccccccc" });
            definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd" });
            definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee" });
            definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff" });
            definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg" });
            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj" });

            Definitions = definitions;

            //Bouton Toutes
            Toutes = new RelayCommand(ToutesDefinitions);
            //Bouton Locales
            Locales = new RelayCommand(DefinitionsLocales);
            //Bouton Roaming
            Roaming = new RelayCommand(DefinitionsRoaming);
            //Bouton Publiques
            Publiques = new RelayCommand(DefinitionsPubliques);
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }



        //Afficher toutes les définitions
        private void ToutesDefinitions()
        {
            TypeDefinitions = "Toutes";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa" });
            definitions.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb" });
            definitions.Add(new Definitions { Mot = "c", Definition = "cccccccccccc" });
            definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd" });
            definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee" });
            definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff" });
            definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg" });
            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj" });

            Definitions = definitions;
        }

        //Afficher toutes les définitions locales
        private void DefinitionsLocales()
        {
            TypeDefinitions = "Locales";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "a", Definition = "aaaaaaaaaaaa" });
            definitions.Add(new Definitions { Mot = "b", Definition = "bbbbbbbbbbbb" });
            definitions.Add(new Definitions { Mot = "c", Definition = "cccccccccccc" });

            Definitions = definitions;
        }

        //Afficher toutes les définitions roaming
        private void DefinitionsRoaming()
        {
            TypeDefinitions = "Roaming";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "d", Definition = "dddddddddddd" });
            definitions.Add(new Definitions { Mot = "e", Definition = "eeeeeeeeeeee" });
            definitions.Add(new Definitions { Mot = "f", Definition = "ffffffffffff" });
            definitions.Add(new Definitions { Mot = "g", Definition = "gggggggggggg" });

            Definitions = definitions;
        }

        //Afficher toutes les définitions publiques
        private void DefinitionsPubliques()
        {
            TypeDefinitions = "Publiques";

            definitions.Clear();

            definitions.Add(new Definitions { Mot = "h", Definition = "hhhhhhhhhhhh" });
            definitions.Add(new Definitions { Mot = "i", Definition = "iiiiiiiiiiii" });
            definitions.Add(new Definitions { Mot = "j", Definition = "jjjjjjjjjjjj" });

            Definitions = definitions;
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        
    }
}
