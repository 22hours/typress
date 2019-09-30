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
    class SocketHandler
    {

        public static DataPacket nowPacket = null, packet = null;
        public static IFormatter formatter = null;
        public static NetworkStream stream = null;
        public static Socket serverLogin, serverMain, serverCB, serverPrint
            , clientLogin, clientMain, clientCB, clientPrint;

        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
        //public const int sPort = 5000;
        public static Thread PrintHooker = null;
        public static Thread ServerOpener = null;


        public static ViewHandler ViewManager = null;
        public static SocketHandler SocketManager = null;
        public static ThreadHandler ThreadManager = null;

        public SocketHandler()
        {

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



        public void ServerOpenLogin(object port)
        {
            while (true)
            {
                try
                {
                    IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                    IPEndPoint serverEndPoint = new IPEndPoint(serverIP, (int)port);

                    serverLogin = new Socket(
                      AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);

                    serverLogin.Bind(serverEndPoint);
                    serverLogin.Listen(10);
                    Console.WriteLine("****서버(로그인)대기중*****");
                    // 항시 연결대기중.

                    clientLogin = serverLogin.Accept();
                    Console.WriteLine("****서버(로그인)~클라이언트(로그인) 연결완료.**");

                    ReceivePacketFromClient(); // 수신대기
                    SendPacketFromServerToLogin(); // 송신

                    // 클라이언트(로그인)의 "종료"하는 시점을 알아야 한다.!!
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
                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverLogin = null;
                    clientLogin = null;
                }
            }
        }

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

                    ReceivePacketFromClient(); // 수신대기
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
                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverCB = null;
                    clientCB = null;
                }
            }
        }

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

                    ReceivePacketFromClient();
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
                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverMain = null;
                    clientMain = null;
                }
            }
        }

        public void ServerOpenPrint(object port)
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
                    getbyte = setbyte = null;
                    getbyte = new byte[1024];
                    setbyte = new byte[1024];
                    serverPrint = null;
                    clientPrint = null;
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

            for (int k = 0; k < 3; k++)
            {
                p.rankers[k].Usage = _rankers[k].Usage * (-1);
                p.rankers[k].Name = _rankers[k].Name;
            }

            _rankers = null;
            rdr2.Close();
            return p;
        }
    }

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
}
