using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLite;
using Wiktionary.Models;
using Wiktionary.Navigation;
using Wiktionary.Views;

namespace Wiktionary.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            CreateDatabase();//Création de la base de données
            RechercherDefinition = new RelayCommand(AfficherRechercherDefinition);//Bouton Rechercher
            ListeDefinitions = new RelayCommand(AfficherListeDefinitions);//Bouton Liste
            AjouterDefinitions = new RelayCommand(AfficherAjoutDefinitions);//Bouton Ajouter
            Parametrer = new RelayCommand(AfficherParametres);//Bouton Paramétrer    
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

        public async void CreateDatabase()
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");
            await connection.CreateTableAsync<DefinitionsTable>();
        }
    }
}