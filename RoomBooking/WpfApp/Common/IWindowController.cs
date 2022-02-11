using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Common
{
    public interface IWindowController
    {

        void ShowWindow(BaseViewModel viewModel);

        void ShowDialog(BaseViewModel viewModel);
        void CloseWindow();
    }
}
