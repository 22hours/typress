using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberMainView.M
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

        public DataPacket() { }
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
