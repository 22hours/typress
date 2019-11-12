using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Nektra.Deviare2;

namespace MyService
{
    delegate bool StartPrintPageEvent(IntPtr hPrinter);

    class DvPrinter
    {
        int PageCnt;
        string num;
        NktSpyMgr _spyMgr;
        NktProcess _process;
        NktHook hookPrinterStart, hookPage, hookPrinterEnd;
        bool IsWorkedPrintStart = false, IsWorkedPrintEnd = false;

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


            //System.Diagnostics.Debugger.Launch();

            hookPrinterStart = _spyMgr.CreateHook("spoolsv.exe!PrvStartDocPrinterW", (int)eNktHookFlags.flgOnlyPreCall);
            hookPrinterStart.OnFunctionCalled += OnFunctionCalledPrinterStart;
            hookPage = _spyMgr.CreateHook("spoolsv.exe!PrvStartPagePrinter", (int)eNktHookFlags.flgOnlyPreCall);
            hookPage.OnFunctionCalled += OnFunctionCalledPrintPage;
            hookPrinterEnd = _spyMgr.CreateHook("spoolsv.exe!PrvEndDocPrinter", (int)eNktHookFlags.flgOnlyPreCall);
            hookPrinterEnd.OnFunctionCalled += OnFunctionCalledPrinterEnd;


            hookPrinterStart.Hook(true);
            hookPrinterStart.Attach(_process, true);
            hookPage.Hook(true);
            hookPage.Attach(_process, true);
            hookPrinterEnd.Hook(true);
            hookPrinterEnd.Attach(_process, true);

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

        private void OnFunctionCalledPrinterStart(INktHook hhook, INktProcess proc, INktHookCallInfo callInfo)
        {
            if (IsWorkedPrintStart)
            {
                IsWorkedPrintStart = false;
                return;
            }
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
                IsWorkedPrintStart = true;
                return;
            }
        }

        private void OnFunctionCalledPrintPage(INktHook hhook, INktProcess proc, INktHookCallInfo callInfo)
        {
            PageCnt++;
            TypressService.eventLog1.WriteEntry("Printer Count Increase Event Page.");
            TypressService.eventLog1.WriteEntry(PageCnt.ToString());
        }

        private void OnFunctionCalledPrinterEnd(INktHook hhook, INktProcess proc, INktHookCallInfo callInfo)
        {
            if (IsWorkedPrintEnd)
            {
                IsWorkedPrintEnd = false;
                return;
            }
            TypressService.eventLog1.WriteEntry("최종출력물 갯수 : ");
            TypressService.eventLog1.WriteEntry(PageCnt.ToString());
            TypressService.eventLog1.WriteEntry("인쇄작업 종료!");
            IsWorkedPrintEnd = true;
        }
    }
}
