using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
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

        Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;


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
            
            
            AddDefList(mot, definition);
            RoamingStorage.Save<Definitions>();

            //roamingSettings.Values["Definitions"] = composite;
            //MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " : " + definition + " a été ajouté avec succès en roaming!", "Félicitation");
            //msgDialog.ShowAsync();
        }

        private void AddDefList(string mot, string definitionAdd)
        {
            Boolean motExiste = false;

            foreach (var item in RoamingStorage.Data)
            {
                Definitions def = item as Definitions;
                if (def.Mot.Equals(mot))
                {
                        motExiste = true;
                        MessageDialog msgDialog = new MessageDialog("Ajout échoué, le mot existe déjà dans la base");
                        msgDialog.ShowAsync();
                }
            }

            if (motExiste == false)
            {
                RoamingStorage.Data.Add(new Definitions { Mot = mot, Definition = definitionAdd, TypeDefinition = "roaming" });
                MessageDialog msgDialog = new MessageDialog("Le mot " + mot + " : " + definitionAdd + " a été ajouté avec succès en roaming!", "Félicitation");
                msgDialog.ShowAsync();
            }
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