using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using TypressPacket;

namespace typressServer
{
    class DAO
    {
        public static DataPacket SelectUsingReader(MySqlConnection cn, DataPacket pk)
        {
            DataPacket p = new DataPacket();
            cn.Open();
            string sql = "SELECT * FROM typress.members WHERE ID='" + pk.Id + "' AND PW='" + pk.Pw + "';";
            //string sql = "SELECT * FROM members";
            MySqlCommand cmd = new MySqlCommand(sql, cn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                p.IsLogin = true;
                p.Id = (string)rdr["ID"];
                p.Pw = (string)rdr["PW"];
                p.Name = (string)rdr["NAME"];
                p.Group = (string)rdr["GROUP"];
                p.Major = (string)rdr["MAJOR"];
                p.Money = (int)rdr["MONEY"];
                p.Memo = (string)rdr["MEMO"];
                p.JoinDate = (DateTime)rdr["JOINDATE"];
            }
            rdr.Close();
            return p;
        }
    }
}
