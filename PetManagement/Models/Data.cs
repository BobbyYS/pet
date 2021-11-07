using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static PetManagement.ViewModels.MESSAGE;

namespace PetManagement.Models
{
    public class Data
    {
        string sconn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        public DataSet GetDataSet( string cmd)
        {
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (SqlConnection sqlConn = new SqlConnection(sconn))
                {
                    sqlConn.Open();
                    SqlCommand sqlComm = new SqlCommand();
                    sqlComm.Connection = sqlConn;
                    sqlComm.CommandTimeout = 30000;
                    sqlComm.CommandText = cmd;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
                    adapter.Fill(ds, "temp");
                    return ds;
                }
            }
            catch (SqlException e)
            {
                return null;
            }
        }
        public DataTable GetDataTable( string cmd)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection sqlConn = new SqlConnection(sconn))
                {
                    sqlConn.Open();
                    SqlCommand sqlComm = new SqlCommand();
                    sqlComm.Connection = sqlConn;
                    sqlComm.CommandTimeout = 30000;
                    sqlComm.CommandText = cmd;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException e)
            {
                return null;
            }
        }

        /// <summary>
        /// 執行異動資料庫的語句(ex:update,insert...)
        /// </summary>
        /// <param name="sql">SQL語法</param>
        /// <returns></returns>
        public bool ExecChangeData ( string sql)
        {
            try
            {
                System.Data.SqlClient.SqlConnection sqlConnection1 =
                    new System.Data.SqlClient.SqlConnection(sconn);

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();

                return true;

            }
            catch (SqlException e)
            {
                return false;
            }
        }

        /// <summary>
        /// 取得留言資料(LIST)
        /// </summary>
        /// <returns></returns>
        public List<Message> MessageData()
        {
            List<Message> dataList = new List<Message>();
            try
            {
                //string findSql = String.Format("SELECT USER_NAME,MESSAGE FROM FORUM {0},{1}", "", "");
                string findSql = String.Format("SELECT USER_ID,USER_NAME,MESSAGE,IMG,CRT_DT FROM FORUM"+
                    " WHERE IS_PUBLIC=1 AND UP_ID='0'"+
                    " ORDER BY CRT_DT DESC");
                DataTable messageTable =GetDataTable(findSql);
                if (messageTable != null)
                {
                    foreach (DataRow dr in messageTable.Rows)
                    {
                        Message data = new Message();
                        data.USER_ID = dr["USER_ID"].ToString().Trim();
                        data.USER_NAME = dr["USER_NAME"].ToString().Trim();
                        data.MESSAGE = dr["MESSAGE"].ToString().Trim();
                        data.IMG = dr["IMG"].ToString().Trim();
                        data.VIDEO = "";//dr["USER_NAME"].ToString();
                        data.UP_ID = 0; //dr["USER_NAME"].ToString();
                        //data.IS_PUBLIC = true;//dr["USER_NAME"].ToString();
                        data.CRT_DT = DateTime.Parse(dr["CRT_DT"].ToString());
                        //data.MDF_DT = null;//dr["USER_NAME"].ToString();
                        dataList.Add(data);
                    }
                }
            }
            catch (SqlException e)
            {
            }
            return dataList;

        }

    }
}


