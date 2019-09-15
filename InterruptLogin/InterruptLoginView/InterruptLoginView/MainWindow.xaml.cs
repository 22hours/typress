using System;
using System.Collections.Generic;
using System.Linq;
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
namespace InterruptLoginView
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    ///

    public partial class MainWindow : Window
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
        public static TcpClient Client = null;
        public static NetworkStream Stream = null;
        public static IFormatter formatter = null;
        public MainWindow()
        {
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
            DataPacket Packet = new DataPacket();
            Packet.Id = id.Text as string;
            Packet.Pw = password.Password as string;
            try
            {
                Client = new TcpClient("127.0.0.1", 13000);
                Stream = Client.GetStream();
                formatter = new BinaryFormatter();
                formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
                formatter.Serialize(Stream, Packet);
                Stream.Close();

            }
            catch ( Exception ea)
            {
                MessageBox.Show("Server가 현재 동작하지 않습니다.");
            }
            finally
            {
            }
        }
        void ClickQuit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
