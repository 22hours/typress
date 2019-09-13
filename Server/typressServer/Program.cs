// NameSpace 선언
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using TypressPacket;
using Login;

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
        public static bool IsLogin = false;
        public static string Name = null, Group = null;

        public static TcpListener Server = null;
        public static TcpClient Client = null;


        [STAThread]
        static void Main(string[] args)
        {
            Server = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
            Server.Start();


            while (true)
            {
                try
                {
                    Console.WriteLine("------------------------");
                    Console.WriteLine("22Hours's Typress Controller.exe \n" +
                        "[ Client Waiting... ]");
                    Console.WriteLine("------------------------");

                    Client = Server.AcceptTcpClient(); // Client wait.
                    Console.WriteLine("Client 연결되었습니다!");

                    while (true)
                    {
                        Console.WriteLine("[ Client\\Login Waiting... ]");
                        if (IsLogin)
                        {
                            SendIsLogin();// Client로 로그인 성공 code 보내기.
                            Console.WriteLine("[ Client\\Login\\Print Waiting... ]");
                            printChk();
                        }
                        else
                        {
                            // Client로 로그인 실패 code 보내기.
                            SendIsLogin();

                            // Client로부터 로그인 값 전달받기
                            ReceiveLogin();
                        }
                         
                         // 위치가 여기가 맞는지?
                    }
                    // Client.Close(); // 위치가 어디인지?
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
                    Console.WriteLine("연결이 종료되었습니다.");
                    Server.Stop();
                }
            }
        }

        public static void SendIsLogin()
        {

            DataPacket Packet = new DataPacket();
            NetworkStream stream = Client.GetStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();

            formatter.Serialize(stream, Packet);
            stream.Close();
        }
        public static void ReceiveLogin()
        {

        }
        public static bool printChk()
        {
            // Waiting Printer Ruest ....
            return true;

        }
    }
}