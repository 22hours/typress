using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TypressPacket;

namespace MemberMainView
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {

        // 이게 Typress.exe에 필요한 정보를 가지고 있을 변수
        DataPacket dp = new DataPacket();

        public string id;
        public string getId()
        {
            return id;
        }
        public void setId(string value)
        {
            id = value;
        }
        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            getDataPacketFromServer();
        }

        public void getDataPacketFromServer()
        {
            // server 에서 datapacket 수신하여 dp 변수에 넣으면 됩니다.
            // dp = server에서 받아온 Datapacekt;
        }

        public DataPacket getNowDataPacket()
        {
            // 각 window instance에서 해당 함수를 호출하여 DataPacket을 불러와 사용함
            return dp;
        }
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }


    }
}
