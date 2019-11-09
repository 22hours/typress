using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nektra.Deviare2;

namespace MyService
{
    class DvPrinter
    {
        int PageCnt;
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

            //hookPrinter = _spyMgr.CreateHook("spoolsv.exe!StartDocPrinterW", (int)eNktHookFlags.flgOnlyPreCall);
            //hookPrinter = _spyMgr.CreateHook("winspool.drv!StartDocPrinterW", (int)eNktHookFlags.flgOnlyPreCall);

            hookPrinter = _spyMgr.CreateHook("spoolsv.exe!PrvStartDocPrinterW", (int)eNktHookFlags.flgOnlyPreCall);
            hookPrinter.OnFunctionCalled += OnFunctionCalledPrinter;

            hookPage = _spyMgr.CreateHook("winspool.drv!StartPagePrinter", (int)eNktHookFlags.flgOnlyPreCall);
            hookPage.OnFunctionCalled += OnFunctionCalledPrintPage;

            hookPrinter.Hook(true);
            hookPrinter.Attach(_process, true);
            hookPage.Hook(true);
            hookPage.Attach(_process, true);

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
            TypressService.eventLog1.WriteEntry("Printer Request Event!");
            PageCnt = 0;
            if (TypressService.packet.IsLogin == false)
            {
                TypressService.eventLog1.WriteEntry("LoginForm 띄움");
                //System.Diagnostics.Debugger.Launch();

                string applicationName = "C:\\Users\\jklh0\\source\\github\\Typress\\InterruptLogin\\InterruptLoginView\\InterruptLoginView\\bin\\x64\\Debug\\InterruptLoginView.exe";
                ApplicationLoader.PROCESS_INFORMATION procInfo;
                ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);

                //Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\InterruptLogin\\InterruptLoginView\\InterruptLoginView\\bin\\x64\\Debug\\InterruptLoginView.exe");
                //P.WaitForExit(); // IsLogin 변수 바꼈는지?
                return;
            }
        }

        private void OnFunctionCalledPrintPage(INktHook hhook, INktProcess proc, INktHookCallInfo callInfo)
        {
            PageCnt++;
            TypressService.eventLog1.WriteEntry("Printer Count Increase Event Page");
            TypressService.eventLog1.WriteEntry(PageCnt.ToString());
        }
    }
}
