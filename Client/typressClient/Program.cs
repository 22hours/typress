using System;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using TypressPacket;

namespace ClientSideSocket
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

    class ClientClass
    {

        public static TcpClient Client = null;
        public static NetworkStream Stream = null;
        public static IFormatter formatter = null;

        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
          
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(" 22Hours's Typress Login.exe ");
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(" Please Enter Key... ");

                    Console.ReadLine();
                    Client = new TcpClient("127.0.0.1", 13000);
                    Console.WriteLine(">> Typress Controller 실행중....");

                    Stream = Client.GetStream();
                    DataPacket Packet = new DataPacket();

                    formatter = new BinaryFormatter();
                    formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();

                    Packet = (LoginPacket)formatter.Deserialize(Stream); // Server로부터 LoginPacket 받아오기

                    if (Packet.IsLogin == true)
                    {
                        // TypressClient.exe : CreateProcess
                        typressClient.TypressUI tpUI = new typressClient.TypressUI(Packet);
                    }
                    else
                    {
                        // LoginUI.exe : CreateProcess
                        typressClient.LoginUI lgUI = new typressClient.LoginUI();
                    }
                    Stream.Close();
                    Client.Close();
                
                }

                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0} \n\n", se.Message.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception : {0} \n\n", ex.Message.ToString());
                }
                finally
                {
                    Console.WriteLine("Typress ver 1.0.0 을 종료합니다.");
                }
                if (true) break;
            }
        }
    }
}

