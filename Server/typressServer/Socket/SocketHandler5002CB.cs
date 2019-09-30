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

                    ReceivePacketFromClientCBClient(); // 수신대기
                    SendPacketFromServerToCB(); // 송신

                    //클라이언트(메인)의 종료시점은 알아야한다.
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
            DataPacket packet = new DataPacket();

            if (nowPacket != null && nowPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
            {
                packet = nowPacket;
            }
            setbyte = ObjectToByteArray(packet);
            clientCB.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
        }

        public static void ReceivePacketFromClientCBClient()
        {

        }
    }
}
