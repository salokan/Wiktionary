using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wiktionary.Controllers;
using Wiktionary.Models;

namespace Wiktionary.ViewModel
{
    public class ModifierDefinitionsViewModel : ViewModelBase, IViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand Modifier { get; set; } //Bouton Modifier
        public ICommand Retour { get; set; } //Bouton Retour

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
            MessageDialog msgDialog = new MessageDialog("Vous avez bien modifié " + MotAModifier + " - " + definition.TypeDefinition + " : " + DefinitionAModifier, "Modification réussie");
            msgDialog.ShowAsync();
            _navigationService.GoBack();
        }

        //Naviguer sur la page précédente
        private void AfficherPagePrecedente()
        {
            _navigationService.GoBack();
        }

        public void GetParameter(object parameter)
        {
            if (parameter != null)
            {
                definition = parameter as Definitions;

                MotAModifier = definition.Mot;
                DefinitionAModifier = definition.Definition;
            }
        }
    }

    public interface IView
    {
        IViewModel ViewModel { get; }
    }

    public interface IViewModel
    {
        void GetParameter(object parameter);
    }
}
