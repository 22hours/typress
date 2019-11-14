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

        public void ServerOpenMain(object port)
        {
            while (true)
            {
                try
                {
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

                    serverMain = new Socket(
                      AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);

                    serverMain.Bind(serverEndPoint);
                    serverMain.Listen(10);
                    
                    TypressService.eventLog1.WriteEntry("서버(Main)대기중");
                    clientMain = serverMain.Accept();
                    TypressService.eventLog1.WriteEntry("서버(Main)~클라이언트(Main) 연결완료");

                    //ReceivePacketFromMainClient();
                    Thread.Sleep(1000);
                    SendPacketFromServerToMain();

                    //클라이언트 
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
                    serverMain.Close();
                    clientMain.Close();

                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverMain = null;
                    clientMain = null;
                }
            }
        }

        public static void SendPacketFromServerToMain()
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
                clientMain.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
            }
            finally
            {
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }

        public static void ReceivePacketFromMainClient() // Packet에 ID, PW만 온다.
        {
            Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
                MySqlConnection conn = new MySqlConnection(strConn);

                clientMain.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
                packet = (DataPacket)ByteArrayToObject(getbyte);

                //DB에 ID와 PW로 접근.
                //if () Access Fail -> Loop.
                //if () Access Success

                packet = SelectUsingReader(conn, packet);
                ThreadHandler.MainPacket = packet;
                if (ThreadHandler.MainPacket.IsLogin == false)
                    TypressService.eventLog1.WriteEntry("로그인되어있지 않습니다.");
            }
            finally
            {
                Monitor.Exit(ThreadHandler.lockObject);
            }
        }
    }
}
