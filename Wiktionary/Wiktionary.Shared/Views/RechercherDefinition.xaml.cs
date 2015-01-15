﻿using Windows.UI.Xaml.Controls;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238
using Windows.UI.Xaml.Navigation;
using Wiktionary.ViewModel;

namespace Wiktionary.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RechercherDefinition : Page, IView
    {
        public RechercherDefinition()
        {
            this.InitializeComponent();
        }

        public ViewModel.IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                this.ViewModel.GetIsBack();
            }
        }  
    }
}
