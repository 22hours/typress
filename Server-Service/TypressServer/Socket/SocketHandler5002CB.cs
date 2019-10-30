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

namespace TypressServer
{
    public partial class SocketHandler
    {
        public void ServerOpenCB(object port)
        {
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

                    Console.WriteLine("****서버(CB)대기중*****");
                    clientCB = serverCB.Accept();
                    Console.WriteLine("****서버(CB)~클라이언트(CB) 연결완료.**");

                    //ReceivePacketFromClientCBClient(); // 수신대기
                    SendPacketFromServerToCB(); // 송신


                    while (ThreadHandler.MainPacket.IsLogin)
                    {
                        ReceivePacketFromClientCBClientDBUpdate();
                        if (ThreadHandler.MainPacket.IsLogin == false)
                        {
                            //로그아웃 요청

                            break;
                        }
                        else
                        {
                            // DB업데이트
                        }
                    }
                }
                catch (SocketException socketEx)
                {
                    ViewHandler.SocketExMessage(socketEx);
                }
                catch (Exception commonEx)
                {
                    ViewHandler.ExMessage(commonEx);
                }
                finally
                {
                    serverCB.Close();
                    clientCB.Close();

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
            Monitor.Enter(ThreadHandler.lockObject);
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
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientCBClient()
        {
            Monitor.Enter(ThreadHandler.lockObject);
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
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientCBClientLogout()
        {
            Monitor.Enter(ThreadHandler.lockObject);
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
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromClientCBClientDBUpdate()
        {
            Monitor.Enter(ThreadHandler.lockObject);
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

                UpdateUsingReader(conn, packet);
                ThreadHandler.MainPacket = packet;
                if (ThreadHandler.MainPacket.IsLogin == false)
                    Console.WriteLine("현재 로그인이 되어있지 않습니다.\n\n");
                else
                    Console.WriteLine("DB에 업데이트되었습니다.");
            }
            finally
            {
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }
    }
}
