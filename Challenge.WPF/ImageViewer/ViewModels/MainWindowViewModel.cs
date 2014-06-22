using MetroImageViewer.ViewModels;
using Caliburn.Micro;
using MemBus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroImageViewer.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        IBus _bus;
        DeliciousFoodSelectorViewModel _deliciousFoodViewModel;
        NotDeliciousFoodViewModel _notDeliciousFoodViewModel;

        private PropertyChangedBase _selectedViewModel;
        public PropertyChangedBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            {
                _selectedViewModel = value;
                NotifyOfPropertyChange(() => SelectedViewModel);
            }
        }

        /// <summary>
        /// Injected via AutoFac IoC container.
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="deliciousFoodViewModel"></param>
        public MainWindowViewModel(IBus bus,
                                   DeliciousFoodSelectorViewModel deliciousFoodViewModel,
                                   NotDeliciousFoodViewModel notDeliciousFoodViewModel)
        {
            _bus = bus;
            bus.Subscribe(this);
            this.DisplayName = "Challenge.WPF";

            _deliciousFoodViewModel = deliciousFoodViewModel;

            _notDeliciousFoodViewModel = notDeliciousFoodViewModel;

            SelectedViewModel = _deliciousFoodViewModel;

            // Our pub/sub system to keep things loosely coupled.
            bus.Subscribe<NavigateToPageMessage>((message) =>
            {
                switch (message.NavigateToPage)
                {
                    case NavigateToPageMessage.Pages.DeliciousFood:
                        SelectedViewModel = _deliciousFoodViewModel;
                        break;
                    case NavigateToPageMessage.Pages.NotDeliciousFood:
                        SelectedViewModel = _notDeliciousFoodViewModel;
                        break;
                }
            });
        }
    }

    public class NavigateToPageMessage
    {
        public enum Pages
        {
            DeliciousFood,
            NotDeliciousFood
        }

        public Pages NavigateToPage { get; private set; }

        public NavigateToPageMessage(Pages page)
        {
            NavigateToPage = page;
        }
    }
}
