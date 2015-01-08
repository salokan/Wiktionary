using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLite;
using Wiktionary.Controllers;
using Wiktionary.Models;
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

            //Cr�ation de la base de donn�es
            CreateDatabase();

            //Bouton Rechercher
            RechercherDefinition = new RelayCommand(AfficherRechercherDefinition);

            //Bouton Liste
            ListeDefinitions = new RelayCommand(AfficherListeDefinitions);

            //Bouton Ajouter
            AjouterDefinitions = new RelayCommand(AfficherAjoutDefinitions);

            //Bouton Param�trer
            Parametrer = new RelayCommand(AfficherParametres);    
        }

        //Naviguer sur la page de recherche des d�finitions
        private void AfficherRechercherDefinition()
        {
            _navigationService.Navigate(typeof(RechercherDefinition));
        }

        public ICommand RechercherDefinition { get; set; }

        //Naviguer sur la page de liste des d�finitions
        private void AfficherListeDefinitions()
        {
            _navigationService.Navigate(typeof(ListeDefinitions));
        }

        public ICommand ListeDefinitions { get; set; }

        //Naviguer sur la page d'ajout des d�finitions
        private void AfficherAjoutDefinitions()
        {
            _navigationService.Navigate(typeof(AjouterDefinitions));
        }

        public ICommand AjouterDefinitions { get; set; }

        //Naviguer sur la page de param�trage
        private void AfficherParametres()
        {
            _navigationService.Navigate(typeof(Parametrer));
        }

        public ICommand Parametrer { get; set; }

        public async Task<bool> DoesDbExist(string DatabaseName)
        {
            bool dbexist = true;
            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DatabaseName);

            }
            catch
            {
                dbexist = false;
            }

            return dbexist;
        }

        public async void CreateDatabase()
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");
            await connection.CreateTableAsync<DefinitionsTable>();
        }
    }
}