using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypressPacket;
using MyService.Handler.Handler_Thread;

namespace MyService.Handler.Handler_Socket
{
    public partial class SocketHandler
    {
        public void ServerOpenPrint(object port)
        {
            //System.Diagnostics.Debugger.Launch();
            while (true)
            {
                try
                {
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

                    serverPrint = new Socket(
                      AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);

                    serverPrint.Bind(serverEndPoint);
                    serverPrint.Listen(10);
                    
                    TypressService.eventLog1.WriteEntry("서버(Print)대기중");
                    clientPrint = serverPrint.Accept();
                    TypressService.eventLog1.WriteEntry("서버(Print) ~ 클라이언트(Print) 연결완료");

                    ReceivePacketFromClientPrintClient();
                    PrintBit.Set();

                }
                catch (SocketException socketEx)
                {
                    //ViewHandler.SocketExMessage(socketEx);
                }
                catch (Exception commonEx)
                {
                    //ViewHandler.ExMessage(commonEx);
                }
                finally
                {
                    serverPrint.Close();
                    clientPrint.Close();

                    pGetbyte = pSetbyte = null;
                    pGetbyte = new byte[1024];
                    pSetbyte = new byte[1024];
                    serverPrint = null;
                    clientPrint = null;
                }
            }
        }

        public static void SendPacketFromServerToPrint()
        {
            Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                if (ThreadHandler.MainPacket != null && ThreadHandler.MainPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
                {
                    packet = ThreadHandler.MainPacket;
                }
                setbyte = ObjectToByteArray(packet);
                clientPrint.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
            }
            finally
            {
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientPrintClient()
        {
            //Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                PrintedPacket pp = new PrintedPacket();

                //string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
                //MySqlConnection conn = new MySqlConnection(strConn);

                clientPrint.Receive(pGetbyte, 0, pGetbyte.Length, SocketFlags.None);
                pp = (PrintedPacket)ByteArrayToObject(pGetbyte);

                //DB에 ID와 PW로 접근.
                //if () Access Fail -> Loop.
                //if () Access Success

                //packet = SelectUsingReader(conn, packet);
                ThreadHandler.PrintPacket = pp;
                //if (ThreadHandler.MainPacket.IsLogin == false)
                //    TypressService.eventLog1.WriteEntry("현재 로그인이 되어있지 않습니다.");
            }
            finally
            {
                //Monitor.Exit(ThreadHandler.lockObject);
            }
        }
    }
}
