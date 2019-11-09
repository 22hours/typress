using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TypressPacket
{
    [Serializable]
    public struct Ranking{
        public Int32 Usage { get; set; }
        public string Name { get; set; }
        public Ranking(Int32 _usage, string _name) { Usage = _usage; Name = _name; }
    }

    [Serializable]
    public class myReverserClass : IComparer
    {
        //Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(Object _x, Object _y)
        {
             Ranking x = (Ranking)_x;
             Ranking y = (Ranking)_y;
            return x.Usage.CompareTo(y.Usage);
        }

    }

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
        public Ranking[] rankers = new Ranking[3];

        public Ranking[] GetRankers()
        {
            return rankers;
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
