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
using System.Diagnostics;

namespace MyService.Handler.Handler_Socket
{
    public partial class SocketHandler
    {
        public static bool Exit = false;
        public void ServerOpenCB(object port)
        {
            System.Diagnostics.Debugger.Launch();
            while (true)
            {

                try
                {
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

                    serverCB = new Socket(
                      AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);

                    serverCB.Bind(serverEndPoint);
                    serverCB.Listen(10);
                    
                    TypressService.eventLog1.WriteEntry("서버(CB)대기중");
                    clientCB = serverCB.Accept();
                    TypressService.eventLog1.WriteEntry("서버(CB) ~ 클라이언트(CB) 연결완료");

                    //ReceivePacketFromClientCBClient(); // 수신대기
                    SendPacketFromServerToCB(); // 송신

                    //System.Diagnostics.Debugger.Launch();
                    //while (ThreadHandler.MainPacket.IsLogin && !Exit)
                    //{
                        ReceivePacketFromClientCBClientDBUpdate();
                    //}
                }
                catch (SocketException socketEx)
                {
                    TypressService.eventLog1.WriteEntry(socketEx.Message);
                    //ViewHandler.SocketExMessage(socketEx);
                }
                catch (Exception commonEx)
                {
                    TypressService.eventLog1.WriteEntry(commonEx.Message);
                    //ViewHandler.ExMessage(commonEx);
                }
                finally
                {
                    serverCB.Close();
                    clientCB.Close();
                    if (Exit)
                    {
                        TypressService.eventLog1.WriteEntry("출력 지속으로 인한 CB 중단");
                        Exit = false;
                    }
                    TypressService.eventLog1.WriteEntry("서버(CB) ~ 클라이언트(CB) 연결종료");
                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverCB = null;
                    clientCB = null;
                }
            }
        }

        public static void SendPacketFromServerToCB()
        {
            //Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                if (ThreadHandler.MainPacket != null && ThreadHandler.MainPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
                {
                    packet = ThreadHandler.MainPacket;
                }
                setbyte = ObjectToByteArray(packet);
                clientCB.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
            }
            finally
            {
                //Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientCBClient()
        {
            //Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
                MySqlConnection conn = new MySqlConnection(strConn);

                clientCB.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
                packet = (DataPacket)ByteArrayToObject(getbyte);

                //DB에 ID와 PW로 접근.
                //if () Access Fail -> Loop.
                //if () Access Success

                packet = SelectUsingReader(conn, packet);
                ThreadHandler.MainPacket = packet;
                if (ThreadHandler.MainPacket.IsLogin == false)
                    Console.WriteLine("현재 로그인이 되어있지 않습니다.\n\n");
            }
            finally
            {
                //Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientCBClientLogout()
        {
            //Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                clientCB.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
                packet = (DataPacket)ByteArrayToObject(getbyte);

                ThreadHandler.MainPacket = packet;
                //Console.WriteLine("로그아웃 완료");
            }
            finally
            {
                //Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientCBClientDBUpdate()
        {
            //Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
                MySqlConnection conn = new MySqlConnection(strConn);

                clientCB.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
                packet = (DataPacket)ByteArrayToObject(getbyte);

                //DB에 ID와 PW로 접근.
                //if () Access Fail -> Loop.
                //if () Access Success
                //if (packet.Opt == 1)
                //{ //  : 프린트 계속한다는 얘기.
                //    Exit = true;
                //    packet.Opt = 0;
                //    return;
                //}

                // 마일리지, 인쇄수 Update
                while (DvPrinter.PageCntData == 0)
                {
                    Thread.Sleep(1000);
                }

                packet = UpdateMileage(packet, DvPrinter.PageCntData);

                UpdateUsingReader(conn, packet);
                ThreadHandler.MainPacket = packet;
                if (ThreadHandler.MainPacket.IsLogin == false)
                    Console.WriteLine("현재 로그인이 되어있지 않습니다.\n\n");
                else
                    Console.WriteLine("DB에 업데이트되었습니다.");
            }
            finally
            {
                //Monitor.Exit(ThreadHandler.lockObject);
                DvPrinter.PageCntData = 0;
            }
        }

        public static DataPacket UpdateMileage(DataPacket p, int cnt)
        {
            p.Money -= (1000 * cnt);
            p.TotalUsage += (cnt);
            return p;
        }
    }
}
