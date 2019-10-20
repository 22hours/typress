using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TypressServer
{
    public class ViewHandler
    {
        public static void TypressOpen()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("22Hours's Typress Start");
        }

        public static void TypressClose()
        {
            Console.WriteLine("22Hours's Typress Close");
            Console.WriteLine("------------------------");

        }

        public static void PrintHookerOpen()
        {
            Console.WriteLine("Controller.exe (PrintHooker)");
        }

        public static void LoginWaiting()
        {
            Console.WriteLine("Controller.exe (Login)\n" +
                   "[ Client Waiting... ]");
            Console.WriteLine("------------------------");
        }
        
        public static void LoginComplete()
        {
            Console.WriteLine("☆★☆ Client 연결되었습니다! ☆★☆");
        }

        public static void NowStateLogOFF()
        {
            Console.WriteLine("--Server에 로그인 X");
            Console.WriteLine("--Server는 Client로부터 Packet 수신대기중 ... ");
        }

        public static void NowStateLogON()
        {
            Console.WriteLine("--Server에 로그인 O");
            Console.WriteLine("--Server는 Client로부터 Packet 수신대기중 ... ");
        }

        public static void ConnectionExit()
        {
            Console.WriteLine("연결이 종료되었습니다.");
        }

        public static void SocketExMessage(SocketException socketEx)
        {
            Console.WriteLine("[Error]:{0}", socketEx.Message);
        }

        public static void ExMessage(Exception Ex)
        {
            Console.WriteLine("[Error]:{0}", Ex.Message);
        }

        public static void OpenControlViewFromPrint()
        {
            Process P = Process.Start("C:\\Users\\jklh0\\source\\github\\Typress\\ControlBlock\\ControlBlock\\bin\\x64\\Debug\\ControlBlock.exe", "PC");
            //P.WaitForExit();
        }
    }
}
