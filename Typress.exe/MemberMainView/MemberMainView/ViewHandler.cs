using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberMainView
{
    class ViewHandler
    {
        public static void OpenControlViewFromPrint()
        {
            Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\ControlBlock\\ControlBlock\\bin\\x64\\Debug\\ControlBlock.exe", "PC");
            //P.WaitForExit();
        }


        public static void OpenMainViewFromWindow()
        {
            Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\Typress.exe\\MemberMainView\\MemberMainView\\bin\\x64\\Debug\\MemberMainView.exe", "WM");
            //P.WaitForExit();
        }

        public static void OpenLoginViewFromMain()
        {
            Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\InterruptLogin\\InterruptLoginView\\InterruptLoginView\\bin\\x64\\Debug\\InterruptLoginView.exe", "WM");
            //P.WaitForExit();
        }
    }
}
