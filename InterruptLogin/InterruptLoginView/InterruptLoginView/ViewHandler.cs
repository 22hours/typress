using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterruptLoginView
{
    class ViewHandler
    {
        public static void OpenControlViewFromPrint()
        {
            MessageBox.Show("프린터 -> LoginForm -> CB");
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\jklh0\source\github\Typress\InterruptLogin\InterruptLoginView\InterruptLoginView\bin\x64\Debug\ControlBlockView.exe");
            Process P = Process.Start(info);
        }


        public static void OpenMainViewFromWindow()
        {
            MessageBox.Show("OS -> LoginForm -> Main");
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\jklh0\source\github\Typress\InterruptLogin\InterruptLoginView\InterruptLoginView\bin\x64\Debug\MemberMainView.exe");
            Process P = Process.Start(info);

        }
    }
}
