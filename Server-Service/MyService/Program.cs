using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
            System.Diagnostics.Debugger.Launch();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TypressService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
