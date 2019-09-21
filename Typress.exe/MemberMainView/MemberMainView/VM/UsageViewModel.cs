using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypressPacket;

namespace MemberMainView.VM
{
    class UsageViewModel : INotifyPropertyChanged
    {
        DataPacket dp = new DataPacket();

        #region property
        private int _week1;
        private int _week2;
        private int _week3;
        private string rank1;
        private string rank2;
        private string rank3;
        private int rankP1;
        private int rankP2;
        private int rankP3;
        private int _totalCount;
        public int totalCount
        {
            get
            {
                return this._totalCount;
            }
            set
            {
                this._totalCount = value;
                OnPropertyChanged("totalCount");
            }
        }
        public int week1
        {
            get
            {
                return this._week1;
            }
            set
            {
                this._week1 = value;
                OnPropertyChanged("week1");
            }
        }

        public int week2
        {
            get
            {
                return this._week2;
            }
            set
            {
                this._week2 = value;
                OnPropertyChanged("week2");
            }
        }

        public int week3
        {
            get
            {
                return this._week3;
            }
            set
            {
                this._week3 = value;
                OnPropertyChanged("week3");
            }
        }

        public string Rank1 { get => rank1; set { rank1 = value; OnPropertyChanged("Rank1 "); } }
        public string Rank2 { get => rank2; set { rank1 = value; OnPropertyChanged("Rank2 "); } }
        public string Rank3 { get => rank3; set { rank1 = value; OnPropertyChanged("Rank3 "); } }
        public int RankP1 { get => rankP1; set { rankP1 = value; OnPropertyChanged("RankP1 "); } }
        public int RankP2 { get => rankP2; set { rankP2 = value; OnPropertyChanged("RankP2 "); } }
        public int RankP3 { get => rankP3; set { rankP3 = value; OnPropertyChanged("RankP3 "); } }

        #endregion property


        public string strConns = "Server=localhost; Port=3306; Database=Typress; Uid=root;Pwd=123";
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private MySqlConnection getConn()
        {
            string strConn = strConns;
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }

        #region getCountNow
        private void getCountNow(string id)
        {
            totalCount = dp.TotalUsage;
        }
        #endregion

        #region getCount7
        private void getCount7(string id)
        {
            week1 = dp.OneweekUsage;
        }
        #endregion


        #region getCount14
        private void getCount14(string id)
        {
            week2 = dp.TwoweekUsage;
            week2 += week1;
        }
        #endregion

        #region getCount21
        private void getCount21(string id)
        {
            week3 = dp.ThreeWeekUsage;
            week3 += week2;
        }
        #endregion



        #region getCountAll
        private void getCountAll(string id)
        {
            totalCount = dp.TotalUsage;
        }
        #endregion


        public UsageViewModel()
        {
            // app.xaml.cs is singleton 
            // so we use dp in app.xaml.cs 
            dp = ((App)Application.Current).getNowDataPacket();
            string id = dp.Id;

            // 이번주 사용량
            getCount7(id);

            // 지난 2주 사용량
            getCount14(id);

            // 지난 3주 사용량 
            getCount21(id);

            // total 사용량.
            getCountAll(id);
        }
    }
}
