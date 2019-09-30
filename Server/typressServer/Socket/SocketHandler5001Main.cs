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

                    Console.WriteLine("****서버(Main)대기중*****");
                    clientMain = serverMain.Accept();
                    Console.WriteLine("****서버(메인뷰)~클라이언트(메인뷰) 연결완료.**");

                    ReceivePacketFromMainClient();
                    Thread.Sleep(1000);
                    SendPacketFromServerToMain();

                    //클라이언트 
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
            DataPacket packet = new DataPacket();

            if (nowPacket != null && nowPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
            {
                packet = nowPacket;
            }
            setbyte = ObjectToByteArray(packet);
            clientMain.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
        }

        public static void ReceivePacketFromMainClient() // Packet에 ID, PW만 온다.
        {

            string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
            MySqlConnection conn = new MySqlConnection(strConn);

            clientMain.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
            packet = (DataPacket)ByteArrayToObject(getbyte);

            //DB에 ID와 PW로 접근.
            //if () Access Fail -> Loop.
            //if () Access Success

            packet = SelectUsingReader(conn, packet);
            nowPacket = packet;
            if (nowPacket.IsLogin == false)
                Console.WriteLine("현재 로그인이 되어있지 않습니다.\n\n");

        }
    }
}
