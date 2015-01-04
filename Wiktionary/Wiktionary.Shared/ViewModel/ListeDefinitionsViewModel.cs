using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;

namespace Wiktionary.ViewModel
{
    public class ListeDefinitionsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ListeDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        public ICommand Retour { get; set; }
    }
}
