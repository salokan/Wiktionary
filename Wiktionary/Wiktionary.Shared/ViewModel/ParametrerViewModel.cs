using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;

namespace Wiktionary.ViewModel
{
    public class ParametrerViewModel : ViewModelBase, IViewModel
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private readonly INavigationService _navigationService;

        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour
        private string _username;
        public string Username //username à modifier
        {
            get
            {
                return _username;
            }

            set
            {
                if (_username != value)
                {
                    _username = value;
                    RaisePropertyChanged();
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
            localSettings.Values["Username"] = _username;
            MessageDialog msgDialog = new MessageDialog("Le username " + _username + " a été modifié avec succès!", "Félicitation");
            msgDialog.ShowAsync();
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
            InitUserName();
        }

        private void InitUserName()
        {
            if (localSettings.Values["Username"].ToString() != null)
                Username = localSettings.Values["Username"].ToString();
        }
    }
}
