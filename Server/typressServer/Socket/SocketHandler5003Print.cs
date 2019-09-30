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
        public void ServerOpenPrint(object port)
        {
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

                    Console.WriteLine("****서버(Print)대기중*****");
                    clientPrint = serverPrint.Accept();
                    Console.ReadLine();
                    Console.WriteLine("Complete!");
                    //SendPacketFromServerToPrint();

                    //ReceivePacketFromClient();

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
                    serverPrint.Close();
                    clientPrint.Close();

                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
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
            Monitor.Enter(ThreadHandler.lockObject);
            try
            {
                DataPacket packet = new DataPacket();

                string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
                MySqlConnection conn = new MySqlConnection(strConn);

                clientPrint.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
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
    }
}
