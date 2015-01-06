using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;
using Wiktionary.Models;

namespace Wiktionary.ViewModel
{
    public class ParametrerViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour
        private string username;
        public string Username //username à modifier
        {
            get
            {
                return username;
            }

            set
            {
                if (username != value)
                {
                    username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }

        public ParametrerViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            //Bouton Modifier
            Modifier = new RelayCommand(ModifierUsername);
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Ajoute ou modifie le username
        private void ModifierUsername()
        {
            MessageDialog msgDialog = new MessageDialog("Le username " + username + " a été modifié avec succès!", "Félicitation");
            msgDialog.ShowAsync();
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        
    }
}
