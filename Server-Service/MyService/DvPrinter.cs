using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nektra.Deviare2;

namespace MyService
{
    class DvPrinter
    {
        string num;
        NktSpyMgr _spyMgr;
        NktProcess _process;
        NktHook hookPrinter, hookPage;
        public DvPrinter()
        {
            _spyMgr = new NktSpyMgr();
            _spyMgr.Initialize();

        }
        public void DvStart()
        {
            _process = GetProcess("spoolsv.exe");
            if (_process == null)
            {
                Console.WriteLine("spoolsv.exe가 실행이 안됩니다.");
                Environment.Exit(0);
            }

            //hookPrinter = _spyMgr.CreateHook("spoolsv.exe!PrvStartDocPrinterW", (int)(eNktHookFlags.flgRestrictAutoHookToSameExecutable | eNktHookFlags.flgOnlyPreCall));

            hookPrinter = _spyMgr.CreateHook("spoolsv.exe!PrvStartDocPrinterW", (int)eNktHookFlags.flgOnlyPreCall);
            hookPrinter.OnFunctionCalled += OnFunctionCalledPrinter;

            hookPrinter.Hook(true);
            hookPrinter.Attach(_process, true);

        }
        public void DvPageCount()
        {

        }
        private NktProcess GetProcess(string proccessName)
        {
            NktProcessesEnum enumProcess = _spyMgr.Processes();
            NktProcess tempProcess = enumProcess.First();
            while (tempProcess != null)
            {
                if (tempProcess.Name.Equals(proccessName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return tempProcess;
                }
                tempProcess = enumProcess.Next();
            }
            return null;
        }

        private void OnFunctionCalledPrinter(INktHook hhook, INktProcess proc, INktHookCallInfo callInfo)
        {
            TypressService.eventLog1.WriteEntry("Event.");
            if (num == "100")
            {
                TypressService.eventLog1.WriteEntry("출력됩니당.");
                num = "0";
                return;
            }
            if (num == "100")
            {

                TypressService.eventLog1.WriteEntry("에엥?");
            }
        }
    }
}
