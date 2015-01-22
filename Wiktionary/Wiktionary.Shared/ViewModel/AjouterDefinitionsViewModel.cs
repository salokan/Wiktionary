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
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private readonly INavigationService _navigationService;

        public ICommand Locale { get; set; } //Bouton Ajouter Locale
        public ICommand Roaming { get; set; } //Bouton Ajouter Roaming
        public ICommand Publique { get; set; } //Bouton Ajouter Publique
        public ICommand Retour { get; set; } //Bouton Retour

        private string _mot;
        public string Mot //Mot à ajouter
        {
            get
            {
                return _mot;
            }

            set
            {
                if (_mot != value)
                {
                    _mot = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _definition;
        public string Definition //Définition à ajouter
        {
            get
            {
                return _definition;
            }

            set
            {
                if (_definition != value)
                {
                    _definition = value;
                    RaisePropertyChanged();
                }
            }
        }

        //Constructeur
        public AjouterDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Locale = new RelayCommand(AjouterLocale); //Bouton Locale
            Roaming = new RelayCommand(AjouterRoaming); //Bouton Roaming
            Publique = new RelayCommand(AjouterPublique); //Bouton Publique
            Retour = new RelayCommand(AfficherPagePrecedente); //Bouton Retour
        }

        #region Ajouter Définition Locale
        //Ajouter un définition locale
        private async void AjouterLocale()
        {
            if (_mot != null && _definition != null && !_mot.Equals("") && !_definition.Equals(""))
            {
                bool existeDeja = false;
                SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

                var result = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions WHERE Mot = ?", new object[] { _mot });
                foreach (var item in result)
                {
                    if (item.Mot.Equals(_mot))
                        existeDeja = true;
                }

                if (existeDeja)
                {
                    MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " possède déjà une définition en locale", "Attention");
                    msgDialog.ShowAsync();
                }
                else
                {
                    var definitionInsert = new DefinitionsTable
                    {
                        Mot = _mot,
                        Definition = _definition
                    };
                    await connection.InsertAsync(definitionInsert);

                    MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " : " + _definition + " a été ajouté avec succès en local!", "Félicitation");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Le mot et la définition ne doivent jamais être vides.", "Attention");
                msgDialog.ShowAsync(); 
            }   
        }
        #endregion

        #region Ajouter Définition Roaming
        //Ajouter un définition roaming
        private async void AjouterRoaming()
        {
            if (_mot != null && _definition != null && !_mot.Equals("") && !_definition.Equals(""))
            {
                bool motExiste = false;

                foreach (var item in RoamingStorage.Data)
                {
                    Definitions def = item as Definitions;
                    if (def != null && def.Mot.Equals(_mot))
                    {
                        motExiste = true;
                        MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " possède déjà une définition en roaming", "Attention");
                        msgDialog.ShowAsync();
                    }
                }

                if (motExiste == false)
                {
                    RoamingStorage.Data.Add(new Definitions { Mot = _mot, Definition = _definition, TypeDefinition = "roaming" });
                    MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " : " + _definition + " a été ajouté avec succès en roaming!", "Félicitation");
                    msgDialog.ShowAsync();
                }

                await RoamingStorage.Save<Definitions>();
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Le mot et la définition ne doivent jamais être vides.", "Attention");
                msgDialog.ShowAsync(); 
            }  
        }
        #endregion

        #region Ajouter Définition Publique
        //Ajouter un définition publique
        private async void AjouterPublique()
        {
            if (_mot != null && _definition != null && !_mot.Equals("") && !_definition.Equals(""))
            {
                Webservices ws = new Webservices();
                string response = await ws.AddDefinition(_mot, _definition, localSettings.Values["Username"].ToString());

                if (response.Equals("\"Success\""))
                {
                    MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " : " + _definition + " a été ajouté avec succès en publique!", "Félicitation");
                    msgDialog.ShowAsync();
                }
                else
                {
                    MessageDialog msgDialog = new MessageDialog("Le mot " + _mot + " possède déjà une définition en publique", "Attention");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Le mot et la définition ne doivent jamais être vides.", "Attention");
                msgDialog.ShowAsync(); 
            }     
        }
        #endregion

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
           _navigationService.GoBack();
        }
    }
}