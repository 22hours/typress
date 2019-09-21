using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberMainView.VM
{
    class MyPageViewModel: INotifyPropertyChanged
    {
        public string id = "1";
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

        #region getCountNow
        private void getCountNow(string id)
        {
            string strConn = strConns;
            MySqlConnection conn = new MySqlConnection(strConn);
            try
            {
                conn.Open();
                string sql7 = "select sum(count) from print WHERE date(date_ts) BETWEEN subdate(curdate(), date_format(curdate(),'%w')-1)  AND subdate(curdate(), date_format(curdate(),'%w')-7) AND id = '" + id + "'";
                MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
                MySqlDataReader read7 = cmd7.ExecuteReader();
                while (read7.Read())
                {
                    weeknow = Int32.Parse(read7.GetString(0));
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region getCountNow
        private void getCount7(string id)
        {
            string strConn = strConns;
            MySqlConnection conn = new MySqlConnection(strConn);
            try
            {
                conn.Open();
                string sql7 = "select sum(count) from typress.print WHERE date_ts >= curdate() - INTERVAL DAYOFWEEK(curdate())+6 DAY AND date_ts < curdate() - INTERVAL DAYOFWEEK(curdate())-1 DAY AND id = '" + id + "'";
                MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
                MySqlDataReader read7 = cmd7.ExecuteReader();
                while (read7.Read())
                {
                    week1 = Int32.Parse(read7.GetString(0));
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion


        #region getCount14
        private void getCount14(string id)
        {
            string strConn = strConns;
            MySqlConnection conn = new MySqlConnection(strConn);
            try
            {
                conn.Open();
                string sql7 = "select sum(count) from print where date_ts <= date_add(now(), interval-14 day)  AND id = '" + id + "'";
                MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
                MySqlDataReader read7 = cmd7.ExecuteReader();
                while (read7.Read())
                {
                    week2 = Int32.Parse(read7.GetString(0));
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region getCount21
        private void getCount21(string id)
        {
            string strConn = strConns;
            MySqlConnection conn = new MySqlConnection(strConn);
            try
            {
                conn.Open();
                string sql7 = "select sum(count) from print where date_ts <= date_add(now(), interval-21 day)  AND id = '" + id + "'";
                MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
                MySqlDataReader read7 = cmd7.ExecuteReader();
                while (read7.Read())
                {
                    week3 = Int32.Parse(read7.GetString(0));
                    week3 -= (week1 + week2);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region getRank
        private void getRank()
        {
            string strConn = strConns;
            MySqlConnection conn = new MySqlConnection(strConn);
            try
            {
                conn.Open();
                string sql7 = "select sum(count),id from typress.print group by id order by sum(count) desc";
                MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
                MySqlDataReader read7 = cmd7.ExecuteReader();
                string[] rankIDlist = new string[3];
                int[] rankPlist = new int[3];
                int idx = 0;
                while (read7.Read())
                {
                    if (idx == 3) break;
                    rankIDlist[idx] = read7[1] as string;
                    int tmp = read7.GetInt32(0);
                    rankPlist[idx] = tmp;
                    idx += 1;
                }
                Rank1 = rankIDlist[0];
                Rank2 = rankIDlist[1];
                Rank3 = rankIDlist[2];
                RankP1 = rankPlist[0];
                RankP2 = rankPlist[1];
                RankP3 = rankPlist[2];
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
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
            string strConn = "Server=localhost; Port=3306; Database=Typress; Uid=root;Pwd=123";
            MySqlConnection conn = new MySqlConnection(strConn);
            try
            {
                conn.Open();
                string sql7 = "select sum(count) from print where id = '" + id + "'";
                MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
                MySqlDataReader read7 = cmd7.ExecuteReader();
                while (read7.Read())
                {
                    totalCount = Int32.Parse(read7.GetString(0));
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        public MyPageViewModel()
        {
            getCountNow(id);
            getCount7(id);
            getCount14(id);
            getCount21(id);
            getCountAll(id);
            getRank();
        }
    }
}

