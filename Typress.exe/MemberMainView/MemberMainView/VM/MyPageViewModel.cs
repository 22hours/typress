﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemberMainView.M;
using TypressPacket;
using System.Windows;

namespace MemberMainView.VM
{
    class MyPageViewModel: INotifyPropertyChanged
    {
        DataPacket dp = new DataPacket();
        public string id;
        #region property
        private int _weeknow;
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
        public int weeknow
        {
            get
            {
                return this._weeknow;
            }
            set
            {
                this._weeknow = value;
                OnPropertyChanged("weeknow");
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
        public string Rank2 { get => rank2; set { rank2 = value; OnPropertyChanged("Rank2 "); } }
        public string Rank3 { get => rank3; set { rank3 = value; OnPropertyChanged("Rank3 "); } }
        public int RankP1 { get => rankP1; set { rankP1 = value; OnPropertyChanged("RankP1 "); } }
        public int RankP2 { get => rankP2; set { rankP2 = value; OnPropertyChanged("RankP2 "); } }
        public int RankP3 { get => rankP3; set { rankP3 = value; OnPropertyChanged("RankP3 "); } }

        #endregion property
        public string strConns = "Server=localhost; Port=3306; Database=Typress; Uid=root;Pwd=123";
        public event PropertyChangedEventHandler PropertyChanged;

        //select sum(count) from typress.print WHERE date_ts >= curdate() - INTERVAL DAYOFWEEK(curdate())+6 DAY AND date_ts < curdate() - INTERVAL DAYOFWEEK(curdate())-1 DAY



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

        }
        #endregion

        #region getCount21
        private void getCount21(string id)
        {
            week3 = dp.ThreeWeekUsage;
        }
        #endregion

        #region getRank
        private void getRank()
        {
            //다른방식 필요해보임.
        }
        #endregion

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
        #region getCountAll
        private void getCountAll(string id)
        {
            totalCount = dp.TotalUsage;

        }
        #endregion
        public MyPageViewModel()
        {

            // app.xaml.cs is singleton 
            // so we use dp in app.xaml.cs 
            dp = ((App)Application.Current).getNowDataPacket();
            string id = dp.Id;

            // this weeks count
            getCount7(id);

            //2 weeks count
            getCount14(id);

            //3weeks count
            getCount21(id);


            // total Count
            getCountAll(id);
            getRank();
        }
    }
}

