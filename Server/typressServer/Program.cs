// NameSpace 선언
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using TypressPacket;
using System.Threading;
using Nektra.Deviare2;
using System.Diagnostics;

namespace ServerSideSocket
{
    sealed class AllowAllAssemblyVersionsDeserializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;
            String currentAssembly = Assembly.GetExecutingAssembly().FullName;
            assemblyName = currentAssembly;
            typeToDeserialize = Type.GetType(string.Format("{0},{1}", typeName, assemblyName));
            return typeToDeserialize;
        }
    }

    class ServerClass
    {
        public static DataPacket nowPacket = null, packet = null;
        public static IFormatter formatter = null;
        public static NetworkStream stream = null;
        public static Socket server, client;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
        public const int sPort = 5000;


        [STAThread]
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

                    server = new Socket(
                      AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);

                    server.Bind(serverEndPoint);
                    server.Listen(10);

                    Console.WriteLine("------------------------");
                    Console.WriteLine("22Hours's Typress Controller.exe (Server)\n" +
                        "[ Client Waiting... ]");
                    Console.WriteLine("------------------------");

                    client = server.Accept(); // Client wait.
                    Console.WriteLine("☆★☆ Client 연결되었습니다! ☆★☆");
                        
                        while (true)
                    {
                        Console.WriteLine("[ Client\\Login Waiting... \n");

                        Console.WriteLine("Server -> Client : Login 여부를 전송합니다.");
                        SendPakcetToClient(); // <- setbyte

                        if (nowPacket != null && nowPacket.IsLogin)
                        {
                            Console.WriteLine("--Server에 로그인 O ");
                            Console.WriteLine("[ Client\\Login\\Typress... ]");
                            //Client : TypressUI
                            printChk();
                        }
                        else 
                        {
                            Console.WriteLine("--Server에 로그인 X");
                            Console.WriteLine("--Server는 Client로부터 Packet 수신대기중 ... ");
                            //Client : LoginUI

                            ReceivePacketFromClient(); // <- getbyte
                        }
                        getbyte = new byte[1024];
                        setbyte = new byte[1024];
                    }
                }
                catch (SocketException socketEx)
                {
                    Console.WriteLine("[Error]:{0}", socketEx.Message);
                }
                catch (Exception commonEx)
                {
                    Console.WriteLine("[Error]:{0}", commonEx.Message);
                }
                finally
                {
                    client.Close();
                    stream.Close();
                    
                    Console.WriteLine("연결이 종료되었습니다.");
                    server.Close();
                }
            }
        }

        public static void SendPakcetToClient()
        {
            DataPacket packet = new DataPacket(); 

            if (nowPacket != null && nowPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
            {
                packet = nowPacket;
            }
            setbyte = ObjectToByteArray(packet);
            client.Send(setbyte, 0, setbyte.Length, SocketFlags.None);

        }
        public static void ReceivePacketFromClient() // Packet에 ID, PW만 온다.
        {
         
            client.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
            packet = (DataPacket)ByteArrayToObject(getbyte);

            //DB에 ID와 PW로 접근.
            //if () Access Fail -> Loop.
            //if () Access Success

            packet.IsLogin = true;
            packet.Name = "종원";
            packet.Group = "F.A.N";
            packet.Major = "CSIE";
            nowPacket = packet;

        }
        public static void printChk()
        {
            // Waiting Printer Ruest ....
            Console.WriteLine("☆★☆ PrintLogger가 시작되었습니다. ☆★☆");
            PrintLogger PL = new PrintLogger();
            Console.ReadLine();
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }
}