using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Common
{

        public class BaseViewModel : NotifyPropertyChanged
        {
            public IWindowController Controller { get; }

            public BaseViewModel(IWindowController controller)
            {
                Controller = controller;
            }
        }

}
