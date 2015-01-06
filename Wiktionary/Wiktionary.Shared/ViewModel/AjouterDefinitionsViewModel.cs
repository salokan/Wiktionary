using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;

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

            Locale = new RelayCommand(AjouterLocale);
            Roaming = new RelayCommand(AjouterRoaming);
            Publique = new RelayCommand(AjouterPublique);
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Ajouter un définition locale
        private void AjouterLocale()
        {
            MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " : " + definition + " a été ajouté avec succès en local!", "Félicitation");
            msgDialog.ShowAsync();
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
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }
    }
}
