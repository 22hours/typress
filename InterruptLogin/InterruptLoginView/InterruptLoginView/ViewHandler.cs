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
            Process P = Process.Start("ControlBlock.exe");
        }


        public static void OpenMainViewFromWindow()
        {
            MessageBox.Show("OS -> LoginForm -> Main");
            Process P = Process.Start("MemberMainView.exe");

        }
    }
}
