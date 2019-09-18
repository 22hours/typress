using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Nektra.Deviare2;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace ServerSideSocket
{
    public partial class PrintLogger
    {
        private PrintDocument pManager = null; 
        private NktSpyMgr _spyMgr;
        private NktProcess _process;
        private int cnt = 0;

        public PrintLogger()
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
            // if (InvokeRequired)
            //     BeginInvoke(new OutputDelegate(Output), strOutput);
            // else
            // {
            //     textOutput.AppendText(strOutput);
            // }

            cnt++;
            // if (cnt == 3) MessageBox.Show("Printer 출력전 Interrupt!!");
            if (cnt == 3)
            {
                OpenView();
                //MessageBox.Show(strOutput + "\n" + "LoginInterrupt View Excuting...");
                MessageBox.Show("인쇄 장수 : "+ pManager.PrinterSettings.Copies);

            }
            if (cnt == 4) cnt = 0;
            // textOutput.AppendText(strOutput);
        }
        private void OpenView()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Users\\jklh0\\source\\github\\Typress\\InterruptLogin\\InterruptLoginView\\InterruptLoginView\\bin\\Release\\InterruptLoginView.exe";
            Process.Start(startInfo);
        }
    }
}
