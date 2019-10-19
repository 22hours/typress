using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using TypressPacket;
using InterruptLoginView.ViewModel;

namespace InterruptLoginView
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
       
        public static Socket socket;
        public static DataPacket packet = new DataPacket();
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];

        public const int sPort = 5000;
        public static string[] strArg = Environment.GetCommandLineArgs();

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

        public MainWindow()
        {
            //Server
            TypressServerConnect();
            //Thread td = new Thread(new ThreadStart(TypressServerConnect));
            //td.Start(this);


            //UI
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);


        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        void ClickLogin(object sender, RoutedEventArgs e)
        {
            string inputId = id.Text as string;
            string inputPw = password.Password as string;

            packet.Id = inputId;
            packet.Pw = inputPw;

            //mv.ExecuteLogin(inputId, inputPw);
            // 패스워드랑 아이디를 string으로 넘깁니다 -> mv는 ViewModel 폴더의 MainWindowViewModel.cs 의 인스턴스입니당~

            try
            {
                Thread.Sleep(1000);
                SendPacketToServer(packet); // 로그인 시도 Packet보내기
               
                ReceivePacketFromServer(); // 성공여부 반환!!

                if (packet.IsLogin)
                {
                    OpenView();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("ID/PW를 다시 확인하세요 :)");
                }
            }               //}
            catch (Exception ea)
            {
                MessageBox.Show("Server Stopped!");
            }
            finally
            {
                // 서버 수신 대기하는 메소드 필요.
                //this.Close();

            }
        }
        void ClickQuit(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        public void TypressServerConnect()
        {
            try
            {
                IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                IPEndPoint serverEndPoint = new IPEndPoint(serverIP, sPort);

                socket = new Socket(
                    AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(serverEndPoint);

                ReceivePacketFromServer();
                if (packet.IsLogin)
                {
                    //MessageBox.Show("이미 로그인되어있습니다. ControlBlock & MainView 호출");
                    OpenView();
                    this.Close();
                }
            }
            catch(SocketException e)
            {
                MessageBox.Show("Server Stopped!");

            }
        }

        public static void OpenView()
        {
            if (strArg.Length <= 1) // window에서 실행.
            {
                ViewHandler.OpenMainViewFromWindow();
            }
            else
            {
                ViewHandler.OpenControlViewFromPrint();
            }
        }

        public static void ReceivePacketFromServer()
        {
            socket.Receive(getbyte, 0,
                getbyte.Length, SocketFlags.None);

            packet = (DataPacket)ByteArrayToObject(getbyte);
        }

        public static void SendPacketToServer(DataPacket tp)
        {
            setbyte = ObjectToByteArray(tp);
            socket.Send(setbyte, 0, setbyte.Length, SocketFlags.None);
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
