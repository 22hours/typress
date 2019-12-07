using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _22hours_print_api.SpoolAPI;

namespace _22hours_print_api.Monitor
{
    class Monitor
    {
        #region Monitor private variables
        private IntPtr _printerHandle = IntPtr.Zero;
        private IntPtr _changeHandle = IntPtr.Zero;
        private string _spoolerName = "Microsoft Print to PDF"; // 어떤 프린터던지 설정을 가져오는 것. 
        private ManualResetEvent _mrEvent = new ManualResetEvent(false); // 대기중인 thread에 하나이상의 이벤트가 발생했음을 알린다.
        private RegisteredWaitHandle _waitHandle = null; // ???
        private PRINTER_NOTIFY_OPTIONS _notifyOptions = new PRINTER_NOTIFY_OPTIONS();
        private Dictionary<int, string> objJobDict = new Dictionary<int, string>();
        private PrintQueue _spooler = null;
        #endregion

        #region PrinterDelegation
        public delegate void PrintJobStatusChanged(object Sender, PrintJobChangeEventArgs e);
        #endregion

        #region PrinterEvent
        public event PrintJobStatusChanged OnJobStatusChange;
        public void EventOccur(object Sender, PrintJobChangeEventArgs e)
        {
            Console.WriteLine("Print Event Occur!");
        }
        #endregion

        #region DLL Import
        // OpenPrinter : 프린터 생성
        [DllImport("winspool.drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(String pPrinterName,
out IntPtr phPrinter,
Int32 pDefault);

        [DllImport("winspool.drv", EntryPoint = "ClosePrinter",
    SetLastError = true,
    ExactSpelling = true,
    CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter
(Int32 hPrinter);

        // FindFirstPrinterChangeNotification : 첫번째 변화감지를 알린다.
        [DllImport("winspool.drv",
        EntryPoint = "FindFirstPrinterChangeNotification",
        SetLastError = true, CharSet = CharSet.Ansi,
        ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstPrinterChangeNotification
                            ([InAttribute()] IntPtr hPrinter,
                            [InAttribute()] Int32 fwFlags,
                            [InAttribute()] Int32 fwOptions,
                            [InAttribute(), MarshalAs(UnmanagedType.LPStruct)] PRINTER_NOTIFY_OPTIONS pPrinterNotifyOptions);

        // FindNextPrinterChangeNotification : 두번째 부터 ~
        [DllImport("winspool.drv", EntryPoint = "FindNextPrinterChangeNotification",
SetLastError = true, CharSet = CharSet.Ansi,
ExactSpelling = false,
CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextPrinterChangeNotification
                    ([InAttribute()] IntPtr hChangeObject,
                     [OutAttribute()] out Int32 pdwChange,
                     [InAttribute(), MarshalAs(UnmanagedType.LPStruct)] PRINTER_NOTIFY_OPTIONS pPrinterNotifyOptions,
                    [OutAttribute()] out IntPtr lppPrinterNotifyInfo);


        //
        [DllImport("winspool.drv", EntryPoint = "StartPagePrinter",
SetLastError = true, CharSet = CharSet.Ansi,
ExactSpelling = false,
CallingConvention = CallingConvention.StdCall)]
        public static extern int StartPagePrinter
            ([InAttribute()] IntPtr hPrinter);
        #endregion

        #region MonitorConstructor
        public Monitor()
        {
            OnPrinterWaiting();
            Start();
        }
        #endregion
        #region PrintStart
        public void Start()
        {
            // out _printerHandle을 통해, 이름이 _spoolerName인 프린터의 제어권을 얻어온다. 
            OpenPrinter(_spoolerName, out _printerHandle, 0);

            if (_printerHandle != IntPtr.Zero)
            {
                //유효한 프린터 핸들을 등록한다. 변경감지 핸들을 생성하여 Printer or PrinterSever 변경사항을 감지할 수 있음.
                    // 1번째 인자 : OpenPrinter에서 반환한 Handle
                    // 2번째 인자 : ADD, WRITE, 등등 프린터에 JOB이 추가됬을 때, 
                    // 3번째 인자 : 0일 때, 2D Printer
                    // 4번째 인자 : 
                _changeHandle = FindFirstPrinterChangeNotification(_printerHandle, (int)PRINTER_CHANGES.PRINTER_CHANGE_JOB, 0, _notifyOptions);

                // _mrEvent.Handle : 쓰레드에 1개이상의 이벤트가 발생했음을 알려준다. 
                _mrEvent.Handle = _changeHandle;

                //Now, let us wait for change notification from the printer queue....

                // RegisterWaitForSingleObject
                    // 1번재 인자 : _mrEvent
                        // _mrEvent에 신호가 있으면, Interrupt를 걸어도 되겠다!
                        // _mrEvent에 신호가 없으면, 계속 대기
                    // 2번째 인자 : WaitOrTimerCallback
                        // 계속 실행할 인자
                    // 3번째 인자 : 다음 callback함수 얼마간격으로 호출할건지 : -1은 곧바로.
                _waitHandle = ThreadPool.RegisterWaitForSingleObject(_mrEvent, new WaitOrTimerCallback(PrinterNotifyWaitCallback), _mrEvent, -1, true);
            }
            
            _spooler = new PrintQueue(new PrintServer(), _spoolerName);
            foreach (PrintSystemJobInfo psi in _spooler.GetPrintJobInfoCollection())
            {
                objJobDict[psi.JobIdentifier] = psi.Name;
            }
           
        }
        #endregion

        #region PrintStop
        public void Stop()
        {
            if (_printerHandle != IntPtr.Zero)
            {
                ClosePrinter((int)_printerHandle);
                _printerHandle = IntPtr.Zero;
            }
        }
        #endregion



        public void OnPrinterWaiting()
        {
            OnJobStatusChange += new PrintJobStatusChanged(EventOccur);
        }

        #region Callback Function
        public void PrinterNotifyWaitCallback(Object state, bool timedOut)
        {
            //Console.WriteLine("Callback 발동!!!!!");
            if (_printerHandle == IntPtr.Zero) return;
            #region read notification details
            _notifyOptions.Count = 1;
            int pdwChange = 0;
            IntPtr pNotifyInfo = IntPtr.Zero;
            bool bResult = FindNextPrinterChangeNotification(_changeHandle, out pdwChange, _notifyOptions, out pNotifyInfo);

            Console.WriteLine("pdwChange : {0} / pNotifiyInfo : {1}", pdwChange, pNotifyInfo);
            //If the Printer Change Notification Call did not give data, exit code
            if ((bResult == false) || ((pNotifyInfo.ToInt64()) == 0)) return;

            //If the Change Notification was not relgated to job, exit code
            //Job이 ADDJOB,  이딴게 맞는지 한번 보는 것!! 
            bool bJobRelatedChange = ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB) ||
                                     ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_SET_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_SET_JOB) ||
                                     ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_DELETE_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_DELETE_JOB) ||
                                     ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_WRITE_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_WRITE_JOB);
            if (!bJobRelatedChange) return;
            #endregion

            #region Interrupt Catch!!!!!!
            if(bJobRelatedChange == ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB)){
                Console.WriteLine("☆☆☆☆☆☆☆☆ Login Auth ☆☆☆☆☆☆☆☆");
                // if(로그인 여부 -> Login 성공 -> CB) 프린터 계속 진행.
                // if(종료) 함수종료. 
                
            }
            #endregion


            #region populate Notification Information
            //Now, let us initialize and populate the Notify Info data
            PRINTER_NOTIFY_INFO info = (PRINTER_NOTIFY_INFO)Marshal.PtrToStructure(pNotifyInfo, typeof(PRINTER_NOTIFY_INFO));
            Int64 pData = pNotifyInfo.ToInt64() + Marshal.SizeOf(typeof(PRINTER_NOTIFY_INFO));
            PRINTER_NOTIFY_INFO_DATA[] data = new PRINTER_NOTIFY_INFO_DATA[info.Count];
            for (uint i = 0; i < info.Count; i++)
            {
                data[i] = (PRINTER_NOTIFY_INFO_DATA)Marshal.PtrToStructure((IntPtr)pData, typeof(PRINTER_NOTIFY_INFO_DATA));
                pData += Marshal.SizeOf(typeof(PRINTER_NOTIFY_INFO_DATA));
            }
            #endregion

            #region iterate through all elements in the data array
            for (int i = 0; i < data.Count(); i++)
            {

                if ((data[i].Field == (ushort)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_STATUS) &&
                     (data[i].Type == (ushort)PRINTERNOTIFICATIONTYPES.JOB_NOTIFY_TYPE)
                    )
                {
                    JOBSTATUS jStatus = (JOBSTATUS)Enum.Parse(typeof(JOBSTATUS), data[i].NotifyData.Data.cbBuf.ToString());
                    int intJobID = (int)data[i].Id;
                    string strJobName = "";
                    PrintSystemJobInfo pji = null;
                    try
                    {
                        _spooler = new PrintQueue(new PrintServer(), _spoolerName);
                        pji = _spooler.GetJob(intJobID);
                        if (!objJobDict.ContainsKey(intJobID))
                            objJobDict[intJobID] = pji.Name;
                        strJobName = pji.Name;

                        if (bJobRelatedChange == ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB))
                        {
                            Console.WriteLine("★★★★★★★★ Print Out [First] ★★★★★★★★");
                            // Print 시작 ~ 종료까지 Thread 대기 걸고.
                            // Print Real Page Count 넣어야 한다.
                            pji.Cancel();
                        }
                        Console.WriteLine("삭제");
                        pji.Cancel();
                    }
                    catch
                    {
                        pji = null;
                        objJobDict.TryGetValue(intJobID, out strJobName);
                        if (strJobName == null) strJobName = "";
                    }

                    if (OnJobStatusChange != null)
                    {
                        //Let us raise the event
                        OnJobStatusChange(this, new PrintJobChangeEventArgs(intJobID, strJobName, jStatus, pji));
                      

                    }
                }
            }
            #endregion
            if (bJobRelatedChange == ((pdwChange & PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB) == PRINTER_CHANGES.PRINTER_CHANGE_ADD_JOB))
            {
                Console.WriteLine("★★★★★★★★ Print Out [Second] ★★★★★★★★");
                // Print 시작 ~ 종료까지 Thread 대기 걸고.
                // Print Real Page Count 넣어야 한다.
            }

            #region reset the Event and wait for the next event
            _mrEvent.Reset();
            _waitHandle = ThreadPool.RegisterWaitForSingleObject(_mrEvent, new WaitOrTimerCallback(PrinterNotifyWaitCallback), _mrEvent, -1, true);
            #endregion
        }
        #endregion

    }
}
