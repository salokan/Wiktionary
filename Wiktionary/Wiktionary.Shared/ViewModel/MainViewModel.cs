using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;
using Wiktionary.Views;

namespace Wiktionary.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            RechercherDefinition = new RelayCommand(AfficherRechercherDefinition);
            ListeDefinitions = new RelayCommand(AfficherListeDefinitions);
            AjouterDefinitions = new RelayCommand(AfficherAjoutDefinitions);
            Parametrer = new RelayCommand(AfficherParametres);    
        }

        //Naviguer sur la page de recherche des définitions
        private void AfficherRechercherDefinition()
        {
            _navigationService.Navigate(typeof(RechercherDefinition));
        }

        public ICommand RechercherDefinition { get; set; }

        //Naviguer sur la page de liste des définitions
        private void AfficherListeDefinitions()
        {
            _navigationService.Navigate(typeof(ListeDefinitions));
        }

        public ICommand ListeDefinitions { get; set; }

        //Naviguer sur la page d'ajout des définitions
        private void AfficherAjoutDefinitions()
        {
            _navigationService.Navigate(typeof(AjouterDefinitions));
        }

        public ICommand AjouterDefinitions { get; set; }

        //Naviguer sur la page de paramétrage
        private void AfficherParametres()
        {
            _navigationService.Navigate(typeof(Parametrer));
        }

        public ICommand Parametrer { get; set; }
    }
}