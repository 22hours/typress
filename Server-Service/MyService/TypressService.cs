using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using System.Timers;
using System.Threading;
using MyService.Handler.Handler_Socket;
using MyService.Handler.Handler_Thread;
using TypressPacket;

namespace MyService
{
    public partial class TypressService : ServiceBase
    {
        public static EventLog eventLog1;
        //private bool isLogin;
        public static DataPacket packet;
        static public int eventId = 0;
        private DvPrinter dv;

        public TypressService()
        {
            InitializeComponent();

            packet = new DataPacket();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Typress Start!");

            #region MainThread Timer
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
            //System.Diagnostics.Debugger.Launch();
            #endregion

            #region ThreadHandler Start
            ThreadHandler th = new ThreadHandler();
            #endregion
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Typress Exit!");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Typress Continue!");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            eventLog1.WriteEntry("Monitoring Typress", EventLogEntryType.Information, eventId++);
        }
    }
}
