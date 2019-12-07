using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _22hours_print_api.Monitor;
using _22hours_print_api.SpoolAPI;

namespace _22hours_print_api.Monitor
{
    class Program
    {
        public static AutoResetEvent Ev = new AutoResetEvent(false);
        public static AutoResetEvent Func1 = new AutoResetEvent(false);
        public static AutoResetEvent Func2 = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Monitor M = new Monitor();
            Ev.WaitOne();
        }
    }
}
