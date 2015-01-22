using System.Windows.Input;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLite;
using Wiktionary.Controllers;
using Wiktionary.Models;

namespace Wiktionary.ViewModel
{
    public class ModifierDefinitionsViewModel : ViewModelBase, IViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour

        private string _motDeBase = "";

        private string _motAModifier;
        public string MotAModifier //TextBlock du mot à modifier
        {
            get
            {
                return _motAModifier;
            }

            set
            {
                if (_motAModifier != value)
                {
                    _motAModifier = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _definitionAModifier;
        public string DefinitionAModifier //TextBlock de la définition à modifier
        {
            get
            {
                return _definitionAModifier;
            }

            set
            {
                if (_definitionAModifier != value)
                {
                    _definitionAModifier = value;
                    RaisePropertyChanged();
                }
            }
        }

        // Définition à modifier que l'on récupère de la page précédente
        private Definitions _definition; 

        //Constructeur
        public ModifierDefinitionsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            //Bouton Retour
            Modifier = new RelayCommand(ModifierDefinition);
            //Bouton Retour
            Retour = new RelayCommand(AfficherPagePrecedente);
        }

        //Modifier la définition
        private void ModifierDefinition()
        {
            if (_definition.TypeDefinition.Equals("locale"))
            {
                ModificationLocale();
            }
            else if (_definition.TypeDefinition.Equals("roaming"))
            {
                ModificationRoaming();
            }
            else if (_definition.TypeDefinition.Equals("publique"))
            {
                ModificationPublique();
            }
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        //On modifie la définition dans la base
        private async void ModificationLocale()
        {
            bool existeDeja = false;
            int id = 0;
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            if (!MotAModifier.Equals(_motDeBase))
            {
                var result = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions WHERE Mot = ?", new object[] { MotAModifier });
                foreach (var item in result)
                {
                    if (item.Mot.Equals(MotAModifier))
                        existeDeja = true;
                }
            }
            

            if (existeDeja)
            {
                MessageDialog msgDialog = new MessageDialog("Le mot " + MotAModifier + " possède déjà une définition en locale", "Attention");
                msgDialog.ShowAsync();
            }
            else
            {
                var result2 = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions WHERE Mot = ?", new object[] { _motDeBase });
                foreach (var item in result2)
                {
                    id = item.id;
                }

                var definitionUpdate = await connection.Table<DefinitionsTable>().Where(x => x.id.Equals(id)).FirstOrDefaultAsync();

                if (definitionUpdate != null)
                {
                    definitionUpdate.Mot = MotAModifier;
                    definitionUpdate.Definition = DefinitionAModifier;
                    await connection.UpdateAsync(definitionUpdate);
                }

                MessageDialog msgDialog = new MessageDialog("Le mot " + MotAModifier + " : " + DefinitionAModifier + " a été modifié avec succès en local!", "Félicitation");
                msgDialog.ShowAsync();
            }
        }

        private async void ModificationRoaming()
        {
            await RoamingStorage.Restore<Definitions>();
            ModDefList(_motDeBase, MotAModifier, DefinitionAModifier);

            await RoamingStorage.Save<Definitions>();
        }

        private void ModDefList(string mot, string motAModifie, string definitionModifie)
        {
            Definitions d = new Definitions();
            bool itemTrouve = false;
            bool motExiste = false;

            foreach (var item in RoamingStorage.Data)
            {
                Definitions def = item as Definitions;
                if (MotAModifier != _motDeBase)
                {
                    if (def != null && def.Mot.Equals(MotAModifier))
                    {
                        motExiste = true;
                        MessageDialog msgDialog = new MessageDialog("Le mot " + motAModifie + " possède déjà une définition en locale", "Attention");
                        msgDialog.ShowAsync();
                    }
                }

                if (def != null && def.Mot.Equals(mot))
                {
                    d = item as Definitions;
                    itemTrouve = true;
                }

            }

            if (itemTrouve && motExiste == false)
            {
                RoamingStorage.Data.Remove(d);
                RoamingStorage.Data.Add(new Definitions { Mot = motAModifie, Definition = definitionModifie, TypeDefinition = "roaming" });
                MessageDialog msgDialog = new MessageDialog("Vous avez bien modifié " + MotAModifier + " - " + _definition.TypeDefinition + " : " + DefinitionAModifier, "Modification réussie");
                msgDialog.ShowAsync();
            }
        }


        //On modifie la définition via le web service
        private async void ModificationPublique()
        {
            Webservices ws = new Webservices();

            //On vérifie si le mot que l'on va ajouter existe déjà dans la liste
            bool existeDeja = false;

            if (!MotAModifier.Equals(_motDeBase))
            {
                string response = await ws.GetDefinition(MotAModifier);
                if (!response.Equals(""))
                    existeDeja = true;
            }

            if (!existeDeja)
            {
                //Pour modifier une définition, on la supprime puis on ajoute la nouvelle
                string response2 = await ws.DeleteDefinition(_motDeBase, "gregnico");
                if (response2.Equals("\"Success\""))
                {
                    string response3 = await ws.AddDefinition(MotAModifier, DefinitionAModifier, "gregnico");

                    if (response3.Equals("\"Success\""))
                    {
                        MessageDialog msgDialog = new MessageDialog("Le mot " + MotAModifier + " : " + DefinitionAModifier + " a été modifié avec succès en publique!", "Félicitation");
                        msgDialog.ShowAsync();
                    }
                    else
                    {
                        MessageDialog msgDialog = new MessageDialog("Le mot " + MotAModifier + " possède déjà une définition en publique, ce qui l'a supprimé", "Attention");
                        msgDialog.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog msgDialog = new MessageDialog("Vous n'avez pas ajouter le mot " + _motDeBase + " donc vous ne pouvez pas le modifier!", "Attention");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                MessageDialog msgDialog = new MessageDialog("Le mot " + MotAModifier + " possède déjà une définition en publique", "Attention");
                msgDialog.ShowAsync();
            }

            
        }

        //Récupère le paramètre contenant la définition à modifier
        public void GetParameter(object parameter)
        {
            if (parameter != null)
            {
                _definition = parameter as Definitions;

                if (_definition != null)
                {
                    _motDeBase = _definition.Mot;
                    MotAModifier = _definition.Mot;
                    DefinitionAModifier = _definition.Definition;
                }
            }
        }

        public void OnNavigatedTo()
        {

        }
    }


    //Interfaces de navigation
    public interface IView
    {
        IViewModel ViewModel { get; }
    }

    public interface IViewModel
    {
        void GetParameter(object parameter);

        void OnNavigatedTo();
    }
}