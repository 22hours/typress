using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypressPacket;

namespace TypressServer
{
    class ThreadHandler
    {
        private const int LOGIN = 5000;
        private const int MAIN = 5001;
        private const int CB = 5002;
        private const int PRINTER = 5003;

        public static object lockObject = new object();
        public static DataPacket MainPacket = new DataPacket();

        public SocketHandler LoginSocketHandler = new SocketHandler();
        public SocketHandler MainSocketHandler = new SocketHandler();
        public SocketHandler CBSocketHandler = new SocketHandler();
        public SocketHandler PrintSocketHandler = new SocketHandler();

        public ThreadHandler()
        {

            Thread LoginListener = new Thread(new ParameterizedThreadStart(LoginSocketHandler.ServerOpenLogin));
            Thread MainListener = new Thread(new ParameterizedThreadStart(MainSocketHandler.ServerOpenMain));
            Thread CBListener = new Thread(new ParameterizedThreadStart(CBSocketHandler.ServerOpenCB));
            Thread PrintListener = new Thread(new ParameterizedThreadStart(PrintSocketHandler.ServerOpenPrint));
            Thread PrintHooker = new Thread(new ThreadStart(PrintHookOpen));

            LoginListener.Start((int)5000);
            MainListener.Start((int)5001);
            CBListener.Start((int)5002);
            PrintListener.Start((int)5003);
            PrintHooker.Start();

            LoginListener.Join();
            MainListener.Join();
            CBListener.Join();
            PrintListener.Join();
            PrintHooker.Join();
        }

        public static void PrintHookOpen()
        {
            while (true)
            {
                // Waiting Printer Ruest ....
                Console.WriteLine("☆★☆ PrintLogger가 시작되었습니다. ☆★☆\n");
                PrintHandler PL = new PrintHandler();
                Console.ReadLine(); // 종료 이벤트 감지.
                Console.WriteLine("☆★☆ PrintLogger가 종료되었습니다. ☆★☆\n");
            }
        }

        public static void OpenControlViewFromPrint()
        {
            Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\ControlBlock\\ControlBlock\\bin\\x64\\Debug\\ControlBlock.exe", "PC");
            //P.WaitForExit();
        }

        public static void OpenMainViewFromWindow()
        {
            Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\Typress.exe\\MemberMainView\\MemberMainView\\bin\\x64\\Debug\\MemberMainView.exe", "WM");
            //P.WaitForExit();
        }
    }
}
