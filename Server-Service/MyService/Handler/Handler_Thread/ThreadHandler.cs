using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypressPacket;
using MyService.Handler.Handler_Socket;

namespace MyService.Handler.Handler_Thread
{
    class ThreadHandler
    {
        private const int LOGIN = 5000;
        private const int MAIN = 5001;
        private const int CB = 5002;
        private const int PRINTER = 5003;

        public static object lockObject = new object();
        public static DataPacket MainPacket = new DataPacket();

        public DvPrinter dv = new DvPrinter();
        public SocketHandler LoginSocketHandler = new SocketHandler();
        public SocketHandler MainSocketHandler = new SocketHandler();
        public SocketHandler CBSocketHandler = new SocketHandler();
        public SocketHandler PrintSocketHandler = new SocketHandler();

        public ThreadHandler()
        {

            #region Make Listener 
            Thread LoginListener = new Thread(new ParameterizedThreadStart(LoginSocketHandler.ServerOpenLogin));
            Thread MainListener = new Thread(new ParameterizedThreadStart(MainSocketHandler.ServerOpenMain));
            Thread CBListener = new Thread(new ParameterizedThreadStart(CBSocketHandler.ServerOpenCB));
            Thread PrintListener = new Thread(new ParameterizedThreadStart(PrintSocketHandler.ServerOpenPrint));
            Thread PrintHooker = new Thread(new ThreadStart(dv.DvStart));
            #endregion

            #region Thread Start
            LoginListener.Start((int)5000);
            MainListener.Start((int)5001);
            CBListener.Start((int)5002);
            PrintListener.Start((int)5003);
            PrintHooker.Start();
            #endregion

           
            //여기 이렇게 처리하는게 맞나???
            // LoginListener.Join();
            // MainListener.Join();
            // CBListener.Join();
            // PrintListener.Join();
            // PrintHooker.Join();
        }
    }
}
