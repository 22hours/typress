using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterruptLoginView
{
    class ViewHandler
    {
        public static void OpenControlViewFromPrint()
        {
            Process P = Process.Start("..\\..\\..\\..\\..\\..\\ControlBlock\\ControlBlock\\bin\\x64\\Debug\\ControlBlock.exe", "PC");
            //P.WaitForExit();
        }


        public static void OpenMainViewFromWindow()
        {
            Process P = Process.Start("..\\..\\..\\..\\..\\..\\MemberMainView\\MemberMainView\\bin\\x64\\Debug\\MemberMainView.exe", "WM");
            //P.WaitForExit();
        }
    }
}
