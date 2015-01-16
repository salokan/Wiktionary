using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
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

        private string motDeBase = "";

        private string motAModifier;
        public string MotAModifier //TextBlock du mot à modifier
        {
            get
            {
                return motAModifier;
            }

            set
            {
                if (motAModifier != value)
                {
                    motAModifier = value;
                    RaisePropertyChanged("MotAModifier");
                }
            }
        }

        private string definitionAModifier;
        public string DefinitionAModifier //TextBlock de la définition à modifier
        {
            get
            {
                return definitionAModifier;
            }

            set
            {
                if (definitionAModifier != value)
                {
                    definitionAModifier = value;
                    RaisePropertyChanged("DefinitionAModifier");
                }
            }
        }

        // Définition à modifier que l'on récupère de la page précédente
        private Definitions definition; 

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
            if (definition.TypeDefinition.Equals("locale"))
            {
                ModificationLocale();
            }
            else if (definition.TypeDefinition.Equals("roaming"))
            {
                ModificationRoaming();
            }
            else if (definition.TypeDefinition.Equals("publique"))
            {
                ModificationPublique();
            }
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        private async void ModificationLocale()
        {
            bool existeDeja = false;
            int id = 0;
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection("Definitions.db");

            if (!MotAModifier.Equals(motDeBase))
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
                var result2 = await connection.QueryAsync<DefinitionsTable>("Select * FROM Definitions WHERE Mot = ?", new object[] { motDeBase });
                foreach (var item in result2)
                {
                    id = item.id;
                }

                var DefinitionUpdate = await connection.Table<DefinitionsTable>().Where(x => x.id.Equals(id)).FirstOrDefaultAsync();

                if (DefinitionUpdate != null)
                {
                    DefinitionUpdate.Mot = MotAModifier;
                    DefinitionUpdate.Definition = DefinitionAModifier;
                    await connection.UpdateAsync(DefinitionUpdate);
                }

                MessageDialog msgDialog = new MessageDialog("Le mot " + MotAModifier + " : " + DefinitionAModifier + " a été modifié avec succès en local!", "Félicitation");
                msgDialog.ShowAsync();
            }
        }

        private async void ModificationRoaming()
        {
            await RoamingStorage.Restore<Definitions>();
            ModDefList(motDeBase, MotAModifier, DefinitionAModifier);

            RoamingStorage.Save<Definitions>();

            //MessageDialog msgDialog = new MessageDialog("Vous avez bien modifié " + MotAModifier + " - " + definition.TypeDefinition + " : " + DefinitionAModifier, "Modification réussie");
            //msgDialog.ShowAsync();
            _navigationService.GoBack();
        }

        private void ModDefList(string mot, string MotAModifie, string definitionModifie)
        {
            Definitions d = new Definitions();
            Boolean itemTrouve = false;
            Boolean motExiste = false;

            foreach (var item in RoamingStorage.Data)
            {
                Definitions def = item as Definitions;
                if (MotAModifier != motDeBase)
                {
                    if (def.Mot.Equals(MotAModifier))
                    {
                        motExiste = true;
                        MessageDialog msgDialog = new MessageDialog("Modification échoué");
                        msgDialog.ShowAsync();
                    }
                }

                if (def.Mot.Equals(mot))
                {
                    d = item as Definitions;
                    itemTrouve = true;
                }

            }

            if (itemTrouve && motExiste == false)
            {
                RoamingStorage.Data.Remove(d);
                RoamingStorage.Data.Add(new Definitions { Mot = MotAModifie, Definition = definitionModifie, TypeDefinition = "roaming" });
                MessageDialog msgDialog = new MessageDialog("Vous avez bien modifié " + MotAModifier + " - " + definition.TypeDefinition + " : " + DefinitionAModifier, "Modification réussie");
                msgDialog.ShowAsync();
            }
        }

        private void ModificationPublique()
        {
            MessageDialog msgDialog = new MessageDialog("Vous avez bien modifié " + MotAModifier + " - " + definition.TypeDefinition + " : " + DefinitionAModifier, "Modification réussie");
            msgDialog.ShowAsync();
            _navigationService.GoBack();
        }

        //Récupère le paramètre contenant la définition à modifier
        public void GetParameter(object parameter)
        {
            if (parameter != null)
            {
                definition = parameter as Definitions;

                motDeBase = definition.Mot;
                MotAModifier = definition.Mot;
                DefinitionAModifier = definition.Definition;
            }
        }

        //Permet de savoir si on accède à cette page via un retour
        public void GetIsBack()
        {

        }
    }

    public interface IView
    {
        IViewModel ViewModel { get; }
    }

    public interface IViewModel
    {
        void GetParameter(object parameter);

        void GetIsBack();
    }
}