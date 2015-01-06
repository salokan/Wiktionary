/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Wiktionary"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Wiktionary.Controllers;

namespace Wiktionary.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<INavigationService, DesignNavigationService>();
            }
            else
            {
                SimpleIoc.Default.Register<INavigationService>(() => new NavigationService());
            }
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public RechercherDefinitionViewModel RechercherDefinition
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RechercherDefinitionViewModel>();
            }
        }

        public ListeDefinitionsViewModel ListeDefinitions
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListeDefinitionsViewModel>();
            }
        }

        public AjouterDefinitionsViewModel AjouterDefinitions
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AjouterDefinitionsViewModel>();
            }
        }

        public ParametrerViewModel Parametrer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ParametrerViewModel>();
            }
        }

         static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RechercherDefinitionViewModel>();
            SimpleIoc.Default.Register<ListeDefinitionsViewModel>();
            SimpleIoc.Default.Register<AjouterDefinitionsViewModel>();
            SimpleIoc.Default.Register<ParametrerViewModel>();
        }
    }
}