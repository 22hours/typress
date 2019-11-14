using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using TypressPacket;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MemberMainView
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {

        // 이게 Typress.exe에 필요한 정보를 가지고 있을 변수
        public static DataPacket dp = new DataPacket();
        public static Socket socket = null;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
        public const int sPort = 5001;

        public string id { get; set; }
        public static bool exitcode = false;
        public static string[] strArg = Environment.GetCommandLineArgs();

        public App()
        {
            try
            {
                this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

                TypressServerConnect();
                //Thread.Sleep(2000);
                if (!dp.IsLogin) {
                    MessageBox.Show("로그인이 필요합니다! [Main]");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("error : {0} [Main]", ex.Message);
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

        public static void TypressServerConnect()
        {
            try
            {
                IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

                socket = new Socket(
                    AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(serverEndPoint);
                //MessageBox.Show("ok11");
                //Thread.Sleep(1000);
                //MessageBox.Show("ok22");
                //SendPacketToServer(dp); // 로그인 여부. 
                getDataPacketFromServer();

                if (!dp.IsLogin)
                {
                    //ViewHandler.OpenLoginViewFromMain();
                    OpenView();
                    exitcode = true;
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show("Server Stopped! [Main]");

            }
            catch (Exception e)
            {
                MessageBox.Show("Don't Work [Main]");

            }
        }


        public static void getDataPacketFromServer()
        {
            socket.Receive(getbyte, 0,
                getbyte.Length, SocketFlags.None);

            dp = (DataPacket)ByteArrayToObject(getbyte);
        }

        public DataPacket getNowDataPacket()
        {
            // 각 window instance에서 해당 함수를 호출하여 DataPacket을 불러와 사용함
            return dp;
        }


        public static void SendPacketToServer(DataPacket tp)
        {
            setbyte = ObjectToByteArray(tp);
            socket.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            MessageBox.Show(errorMessage, "Error [Main]", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
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

        public static void OpenView()
        {
            ViewHandler.OpenLoginViewFromMain(); // 얘는 로그인에서 Main을 키겠지.
        }
    }
}
