using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace ForgeEditor
{
    public interface IMainShell
    {
        Igneel.Windows.Wpf.Canvas3D.Win32Canvas Canvas3D { get; }

        Dispatcher Dispatcher { get; }

        void ShowProgressDialog();

        void HideProgressDialog();
    }
}
