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

    }
}
