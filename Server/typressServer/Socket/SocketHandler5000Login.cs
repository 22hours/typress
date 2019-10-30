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

        public void ServerOpenLogin(object port)
        {
            while (true)
            {
                try
                {
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

                    serverLogin = new Socket(
                      AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);

                    serverLogin.Bind(serverEndPoint);
                    serverLogin.Listen(10);
                    Console.WriteLine("****서버(로그인)대기중*****");
                    // 항시 연결대기중.

                    clientLogin = serverLogin.Accept();
                    Console.WriteLine("****서버(로그인)~클라이언트(로그인) 연결완료.**");

                    Thread.Sleep(2000);
                    //Receive : 연결 햇군!(굳이 Client에서 Send안해도된다.)
                    //SendPacket : 로그인 여부를 Client로 보낸다. 
                    SendPacketFromServerToLogin();
                    Console.WriteLine("로그인폼으로 Packet전달완료.");

                    if (ThreadHandler.MainPacket.IsLogin)
                    {
                        throw new Exception("이미 로그인 되어있습니다.");
                    }

                    //Client 창 로그인창.
                    while (!ThreadHandler.MainPacket.IsLogin)
                    {
                        // 수신대기
                        Console.WriteLine("수신대기");
                        ReceivePacketFromLoginClient();
                        Thread.Sleep(1000);
                        Console.WriteLine("패킷송신");
                        SendPacketFromServerToLogin(); // 송신
                   
                    }
                    // CB를 틀 수 있도록 해야함.
                   

                    // 클라이언트(로그인)의 "종료" : 
                    //     1) 로그인 창을 껐을 때 : 서버에서 예외처리
                    //     2) 로그인을 성공했을 때
                    //      * 끝난 후 다시 항시 대기.
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
                    //serverLogin.Shutdown(SocketShutdown.Both);
                    serverLogin.Close();
                    clientLogin.Close();

                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverLogin = null;
                    clientLogin = null;
                }
            }
        }

        public static void SendPacketFromServerToLogin()
        {
            Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();
                //DataPacket packet = ThreadHandler.MainPacket;

                //if (nowPacket != null && nowPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
                if(ThreadHandler.MainPacket != null && ThreadHandler.MainPacket.IsLogin == true)
                {
                    packet = ThreadHandler.MainPacket;
                }
                setbyte = ObjectToByteArray(packet);
                clientLogin.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
            }
            finally
            {
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromLoginClient() // Packet에 ID, PW만 온다.
        {
            Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();
                string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
                MySqlConnection conn = new MySqlConnection(strConn);

                clientLogin.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
                packet = (DataPacket)ByteArrayToObject(getbyte);

                packet = SelectUsingReader(conn, packet);

                //nowPacket = packet;
                ThreadHandler.MainPacket = packet;
                if (ThreadHandler.MainPacket.IsLogin == false)
                    Console.WriteLine("ID/PW가 잘못되었습니다.\n\n");
            }
            finally
            {
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

    }
}
