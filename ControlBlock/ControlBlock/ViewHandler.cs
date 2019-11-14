using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ControlBlock
{
    class ViewHandler
    {
        public static void OpenControlViewFromPrint()
        {
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\jklh0\source\github\Typress\InterruptLogin\InterruptLoginView\InterruptLoginView\bin\x64\Debug\ControlBlockView.exe");
            Process P = Process.Start(info);
        }


        public static void OpenMainViewFromWindow()
        {
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\jklh0\source\github\Typress\InterruptLogin\InterruptLoginView\InterruptLoginView\bin\x64\Debug\MemberMainView.exe");
            Process P = Process.Start(info);
        }

        public static void OpenLoginViewFromMain()
        {
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Users\jklh0\source\github\Typress\InterruptLogin\InterruptLoginView\InterruptLoginView\bin\x64\Debug\InterruptLoginView.exe");
            Process P = Process.Start(info);
        }
    }
}
