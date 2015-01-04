using System;
using System.Collections.Generic;
using System.Text;

namespace Wiktionary.Controllers
{
    public interface INavigationService
    {
        void Navigate(Type sourcePageType);
        void Navigate(Type sourcePageType, object parameter);
        void GoBack();
    }
}
