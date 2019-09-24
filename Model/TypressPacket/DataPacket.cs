using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypressPacket
{
    [Serializable]
    public class DataPacket
    {
        public bool IsLogin { get; set; }
        public string Id { get; set; }
        public string Pw { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Major { get; set; }
        public Int32 Money { get; set; }
        public string Memo { get; set; }
        public DateTime JoinDate { get; set; }
        public Int32 ThreeWeekUsage {get;set;}
        public Int32 TwoWeekUsage { get; set; }
        public Int32 OneWeekUsage { get; set; }
        public Int32 TotalUsage { get; set; }

        public string[] RankName = new string[20];
        public string[] GetRankName()
        {
            return RankName;
        }
        public Int32[] RankUsage = new Int32[20];
        public Int32[] GetRankUsage()
        {
            return RankUsage;
        }

        public DataPacket() {}
        public DataPacket(string n, string g)
        {
            IsLogin = false;
            Id = null;
            Pw = null;
            Name = n;
            Group = g;
            Major = "C.S.I.E";
            Money = 10000;
            Memo = "잘 부탁드립니다.";
            JoinDate = new DateTime();
        }
    }
}
