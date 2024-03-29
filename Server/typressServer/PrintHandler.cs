﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Nektra.Deviare2;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Management;

namespace TypressServer
{

    public partial class PrintHandler
    {
        private PrintDocument pManager = null; 
        private NktSpyMgr _spyMgr;
        private NktProcess _process;
        private int cnt = 0;

        public PrintHandler()
        {
            // InitializeComponent();

            _spyMgr = new NktSpyMgr();
            _spyMgr.Initialize();
            _spyMgr.OnFunctionCalled += new DNktSpyMgrEvents_OnFunctionCalledEventHandler(OnFunctionCalled);



            GetProcess("spoolsv.exe");
            if (_process == null)
            {
                // MessageBox.Show("Please start \"spoolsv.exe\" before!", "Error");
                Console.WriteLine("spoolsv.exe가 실행이 안되었습니다.");
                Environment.Exit(0);
            }
            Thread WaitHook = new Thread(new ThreadStart(PrintLogger_Load));
            WaitHook.Start();
        }

        //private void PrintLogger_Load(object sender, EventArgs e)
        private void PrintLogger_Load()
        {
            NktHook hook = _spyMgr.CreateHook("spoolsv.exe!PrvStartDocPrinterW", (int)(eNktHookFlags.flgRestrictAutoHookToSameExecutable & eNktHookFlags.flgOnlyPreCall));
            hook.Hook(true);
            hook.Attach(_process, true);
        }

        private bool GetProcess(string proccessName)
        {
            NktProcessesEnum enumProcess = _spyMgr.Processes();
            NktProcess tempProcess = enumProcess.First();
            while (tempProcess != null)
            {
                if (tempProcess.Name.Equals(proccessName, StringComparison.InvariantCultureIgnoreCase) && tempProcess.PlatformBits > 0 && tempProcess.PlatformBits <= IntPtr.Size * 8)
                {
                    _process = tempProcess;
                    return true;
                }
                tempProcess = enumProcess.Next();
            }

            _process = null;
            return false;
        }

        public delegate void OutputDelegate(string strOutput);

        private void OnFunctionCalled(NktHook hook, NktProcess process, NktHookCallInfo hookCallInfo)
        {
            Thread cur_thread = Thread.CurrentThread;

            string strDocument = "Document: ";

            INktParamsEnum paramsEnum = hookCallInfo.Params();

            INktParam param = paramsEnum.First();

            param = paramsEnum.Next();

            param = paramsEnum.Next();

            if (param.PointerVal != IntPtr.Zero)
            {
                INktParamsEnum paramsEnumStruct = param.Evaluate().Fields();
                INktParam paramStruct = paramsEnumStruct.First();

                strDocument += paramStruct.ReadString();
                strDocument += (" thread id : " + cur_thread.ManagedThreadId);
                strDocument += "\n";
            }

            Output(strDocument);
        }

        private void Output(string strOutput)
        {

            cnt++;
            if (cnt == 3)
            {
                GetPrintPages();
                ViewHandler.OpenControlViewFromPrint();
            }
            if (cnt == 4) cnt = 0;
        }

        private int OpenLoginView()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Users\\jklh0\\source\\github\\Typress\\InterruptLogin\\InterruptLoginView\\InterruptLoginView\\bin\\x64\\Debug\\InterruptLoginView.exe";
            Process P = Process.Start(startInfo);
            P.WaitForExit();
            return P.ExitCode; // 로그인 성공
        }

        public static void GetPrintPages()
        {
            string searchQuery = "SELECT * FROM Win32_PrintJob";
            ManagementObjectSearcher searchPrintJobs = new ManagementObjectSearcher(searchQuery);
            ManagementObjectCollection prntJobCollection = searchPrintJobs.Get();
            foreach (ManagementObject prntJob in prntJobCollection)
            {
                try
                {
                    int pages = int.Parse(prntJob.Properties["TotalPages"].Value.ToString());
                    Console.WriteLine("요청된 프린트 page수 : "+ pages);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception getting print jobs: " + ex);
                }
            }
        }
    }
}
