using System.Windows.Input;
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

        private string mot;
        public string Mot //Mot à ajouter
        {
            get
            {
                return mot;
            }

            set
            {
                if (mot != value)
                {
                    mot = value;
                    RaisePropertyChanged("Mot");
                }
            }
        }

        private string definition;
        public string Definition //Définition à ajouter
        {
            get
            {
                return definition;
            }

            set
            {
                if (definition != value)
                {
                    definition = value;
                    RaisePropertyChanged("Definition");
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

            var result = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions WHERE Mot = ?", new object[] { mot });
            foreach (var item in result)
            {
                if (item.Mot.Equals(mot))
                    existeDeja = true;
            }

            if (existeDeja)
            {
                MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " possède déjà une définition en locale", "Attention");
                msgDialog.ShowAsync();
            }
            else
            {
                var DefinitionInsert = new DefinitionsTable()
                {
                    Mot = mot,
                    Definition = definition
                };
                await connection.InsertAsync(DefinitionInsert);

                MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " : " + definition + " a été ajouté avec succès en local!", "Félicitation");
                msgDialog.ShowAsync();
            }
            
        }

        //Ajouter un définition roaming
        private void AjouterRoaming()
        {
            MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " : " + definition + " a été ajouté avec succès en roaming!", "Félicitation");
            msgDialog.ShowAsync();
        }

        //Ajouter un définition publique
        private void AjouterPublique()
        {
            MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " : " + definition + " a été ajouté avec succès en public!", "Félicitation");
            msgDialog.ShowAsync();
        }

        //Naviguer sur la page précédente
        private async void AfficherPagePrecedente()
        {
           _navigationService.GoBack();
        }
    }
}