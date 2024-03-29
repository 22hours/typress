﻿using System;
using System.IO;
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

        public static Socket socket;
        public static DataPacket packet;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
        public static typressClient.TypressUI tpUI = null;
        public static typressClient.LoginUI lgUI = null;

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

                    socket = new Socket(
                        AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(" 22Hours's Typress Login.exe (Client)");
                    Console.WriteLine(" [ Server Searching... ]");
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(" Please [Enter] Key... ");

                    Console.ReadLine();

                    socket.Connect(serverEndPoint);

                    Console.WriteLine("★☆★ Server에 연결되었습니다! ★☆★");
                    Console.WriteLine(">> Typress Controller 실행중....");
                    // Server : LoginCode Transmission
                    // View가 뜨기 전
                    while (true)
                    {
                        Console.WriteLine("--Server로부터 로그인 Code 수신대기중...");

                        ReceivePacketFromServer();
                        Console.WriteLine("--Client <- Server 로그인 Code 수신완료!\n");

                        // Server : 로그인 되어있음.(로그인 계정정보까지 전달해줬음)
                        // UI : TypressUI
                        if (packet.IsLogin == true)
                        {

                            Console.WriteLine("--Code : 로그인되어있는 상태!");
                            // TypressClient.exe : CreateProcess
                            tpUI = new typressClient.TypressUI(packet);
                        }
                        // Server : 로그인 안되어있음.(IsLogin == False)
                        else
                        {

                            Console.WriteLine("--Code : 로그인 안돼있는 상태!");
                            // LoginUI.exe : CreateProcess
                            lgUI = new typressClient.LoginUI();

                            SendPacketToServer();
                        }
                        getbyte = new byte[1024];
                        setbyte = new byte[1024];
                    }
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0} \n\n", se.Message.ToString());
                    socket.Close();
                    Console.WriteLine("Typress ver 1.0.0 을 종료합니다.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception : {0} \n\n", ex.Message.ToString());
                    socket.Close();
                    Console.WriteLine("Typress ver 1.0.0 을 종료합니다.");

                }
            }
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
        public static void ReceivePacketFromServer()
        {
            socket.Receive(getbyte, 0,
                getbyte.Length, SocketFlags.None);

            packet = (DataPacket)ByteArrayToObject(getbyte);
        }
        public static void SendPacketToServer()
        {
            DataPacket tp = lgUI.P;
            setbyte = ObjectToByteArray(tp);
            socket.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
        }
    }
}

