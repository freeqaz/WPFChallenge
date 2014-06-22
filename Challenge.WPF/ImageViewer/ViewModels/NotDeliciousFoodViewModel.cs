using Caliburn.Micro;
using MemBus;
using MetroImageViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Articulate.ViewModels
{
    public class NotDeliciousFoodViewModel : Screen
    {
        private IBus _bus;

        public NotDeliciousFoodViewModel(IBus bus)
        {
            _bus = bus;
        }

        public void GoBack()
        {
            _bus.Publish(new NavigateToPageMessage(NavigateToPageMessage.Pages.DeliciousFood));
        }
    }
}
