using System;

namespace Wiktionary.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type sourcePageType);
        void Navigate(Type sourcePageType, object parameter);
        void GoBack();
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
