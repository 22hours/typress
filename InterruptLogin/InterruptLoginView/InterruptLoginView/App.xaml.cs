using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TypressPacket;

namespace InterruptLoginView
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class App : Application
    {
        DataPacket dp = new DataPacket();


        public void getDataPacketFromServer()
        {
            // server 에서 datapacket 수신하여 dp 변수에 넣으면 됩니다.
            // dp = server에서 받아온 Datapacekt;

            dp.Id = "winterlood";
            dp.Money = Int32.Parse("12000");
            dp.TotalUsage = Int32.Parse("12000");
            dp.OneweekUsage = Int32.Parse("12000");
            dp.TwoweekUsage = Int32.Parse("12000");
            dp.ThreeWeekUsage = Int32.Parse("12000");
        }

        public DataPacket getNowDataPacket()
        {
            // 각 window instance에서 해당 함수를 호출하여 DataPacket을 불러와 사용함
            return dp;
        }
    }
}
