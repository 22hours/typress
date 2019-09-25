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
using MySql.Data.MySqlClient;
using System.Threading;
using Nektra.Deviare2;
using System.Diagnostics;
using System.Collections;

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
        public static Socket serverLogin, serverMain, clientLogin, clientMain;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
        //public const int sPort = 5000;
        public static Thread PrintHooker = null;
        public static Thread ServerOpener = null;

        [STAThread]
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {
                    Console.WriteLine("------------------------");
                    Console.WriteLine("22Hours's Typress");
                    Console.WriteLine("Controller.exe (PrintHooker)");
                    PrintHooker = new Thread(new ThreadStart(PrintHookOpen));
                    PrintHooker.Start();

                    //ServerOpener = new Thread(new ThreadStart(ServerOpen));
                    //ServerOpener.Start();
                    ServerOpenLogin((int)5000);

                    
                    Console.WriteLine("Controller.exe (Server)\n" +
                        "[ Client Waiting... ]");
                    Console.WriteLine("------------------------");


                    clientLogin = serverLogin.Accept();

                    Console.WriteLine("☆★☆ Client 연결되었습니다! ☆★☆");
                        
                    while (true)
                    {
                        Console.WriteLine("[ Client\\Login Waiting... \n");

                        //Console.WriteLine("Server -> Client : Login 여부를 전송합니다.");
                        //SendPakcetToClient(); // <- setbyte

                        if (nowPacket != null && nowPacket.IsLogin)
                        {
                            Console.WriteLine("--Server에 로그인 O ");
                            Console.WriteLine("[ Client\\Login\\Typress... ]");

                            Thread MainViewController = new Thread(new ParameterizedThreadStart(ServerOpenMain));
                            MainViewController.Start((int)5001);

                            Thread.Sleep(1000);
                            OpenMainView();
                            
                            SendPacketFromServerToMain();
                            //Client : TypressUI
                        }
                        else 
                        {
                            Console.WriteLine("--Server에 로그인 X");
                            Console.WriteLine("--Server는 Client로부터 Packet 수신대기중 ... ");
                            //Client : LoginUI

                            ReceivePacketFromClient(); // <- getbyte
                            SendPacketFromServerToLogin();
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
                    clientLogin.Close();
                    stream.Close();
                    
                    Console.WriteLine("연결이 종료되었습니다.");
                    serverLogin.Close();
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

        public static void SendPacketFromServerToLogin()
        {
            DataPacket packet = new DataPacket();

            if (nowPacket != null && nowPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
            {
                packet = nowPacket;
            }
            setbyte = ObjectToByteArray(packet);
            clientLogin.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
        }



        public static void ServerOpenLogin(object port)
        {
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

            serverLogin = new Socket(
              AddressFamily.InterNetwork,
              SocketType.Stream, ProtocolType.Tcp);

            serverLogin.Bind(serverEndPoint);
            serverLogin.Listen(10);
            Console.WriteLine("****서버대기중*****");
        }

        public static void ServerOpenMain(object port)
        {
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

            serverMain = new Socket(
              AddressFamily.InterNetwork,
              SocketType.Stream, ProtocolType.Tcp);

            serverMain.Bind(serverEndPoint);
            serverMain.Listen(10);
            clientMain = serverMain.Accept();
            Console.WriteLine("****서버대기중*****");
            Console.WriteLine("Complete!");
            SendPacketFromServerToMain();
        }


        public static void PrintHookOpen()
        {
            // Waiting Printer Ruest ....
            Console.WriteLine("☆★☆ PrintLogger가 시작되었습니다. ☆★☆\n");
            PrintLogger PL = new PrintLogger();
            Console.ReadLine();
        }

        public static void SendPakcetToClient()
        {
            DataPacket packet = new DataPacket(); 

            if (nowPacket != null && nowPacket.IsLogin == true)  // 이미 로그인 되어있는 경우
            {
                packet = nowPacket;
            }
            setbyte = ObjectToByteArray(packet);
            clientLogin.Send(setbyte, 0, setbyte.Length, SocketFlags.None);

        }
        public static void ReceivePacketFromClient() // Packet에 ID, PW만 온다.
        {

            string strConn = "Server=localhost;Database=typress;UId=typressAdmin;Pwd=typress22hours;Charset=utf8";
            MySqlConnection conn = new MySqlConnection(strConn);

            clientLogin.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
            packet = (DataPacket)ByteArrayToObject(getbyte);

            //DB에 ID와 PW로 접근.
            //if () Access Fail -> Loop.
            //if () Access Success

            packet = SelectUsingReader(conn, packet);
            nowPacket = packet;
            if (nowPacket.IsLogin == false)
                Console.WriteLine("ID/PW가 잘못되었습니다.\n\n");

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

        public static DataPacket SelectUsingReader(MySqlConnection cn, DataPacket pk)
        {
            DataPacket p = new DataPacket();
            cn.Open();
            string sql1 = "SELECT * FROM typress.members WHERE ID='" + pk.Id + "' AND PW='" + pk.Pw + "';";
            string sql2 = "SELECT * FROM typress.rank ORDER BY 'USAGE' DESC;";
            //string sql = "SELECT * FROM members";
            MySqlCommand cmd1 = new MySqlCommand(sql1, cn);
            MySqlCommand cmd2 = new MySqlCommand(sql2, cn);
            MySqlDataReader rdr1 = cmd1.ExecuteReader();

            while (rdr1.Read())
            {
                p.IsLogin = true;
                p.Id = (string)rdr1["ID"];
                p.Pw = (string)rdr1["PW"];
                p.Name = (string)rdr1["NAME"];
                p.Group = (string)rdr1["GROUP"];
                p.Major = (string)rdr1["MAJOR"];
                p.Money = (int)rdr1["MONEY"];
                p.Memo = (string)rdr1["MEMO"];
                p.JoinDate = (DateTime)rdr1["JOINDATE"];
                p.ThreeWeekUsage = (int)rdr1["THREEUSAGE"];
                p.TwoWeekUsage = (int)rdr1["TWOUSAGE"];
                p.OneWeekUsage = (int)rdr1["ONEUSAGE"];
                p.TotalUsage = (int)rdr1["TOTALUSAGE"];
            }
            rdr1.Close();
            MySqlDataReader rdr2 = cmd2.ExecuteReader();

            Ranking[] _rankers = new Ranking[100];
            // Ranking[] rs = new Ranking[3];
            int i = 0;
            while (rdr2.Read())
            {
                _rankers[i].Usage = (int)rdr2["USAGE"] * (-1);
                _rankers[i].Name = (string)rdr2["NAME"];
                i++;
            }
            IComparer myComparer = new myReverserClass();
            Array.Sort(_rankers, myComparer);

            for(int k = 0; k<3; k++)
            {
                p.rankers[k].Usage = _rankers[k].Usage * (-1);
                p.rankers[k].Name = _rankers[k].Name;
            }

            _rankers = null;
            rdr2.Close();
            return p;
        }

        public static int OpenMainView()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Users\\jklh0\\source\\github\\Typress\\Typress.exe\\MemberMainView\\MemberMainView\\bin\\x64\\Debug\\MemberMainView.exe";
            Process P = Process.Start(startInfo);
            P.WaitForExit();
            return P.ExitCode;
        }
    }
}