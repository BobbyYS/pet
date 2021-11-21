using PetManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using static PetManagement.ViewModels.MESSAGE;
using static PetManagement.ViewModels.SYSINFO;

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

    # region 取得主題及留言資料
        /// <summary>
        /// 取得主題及留言資料(LIST)
        /// </summary>
        /// <returns></returns>
        public List<Message> MessageData()
        {
            List<Message> dataList = new List<Message>();
            try
            {
                //string findSql = String.Format("SELECT USER_NAME,MESSAGE FROM FORUM {0},{1}", "", "");
                string findSql = String.Format("SELECT ID,USER_ID,USER_NAME,MESSAGE,IMG,CRT_DT FROM FORUM"+
                    " WHERE IS_PUBLIC=1 AND UP_ID='0'"+
                    " ORDER BY CRT_DT DESC");
                DataTable messageTable =GetDataTable(findSql);
                if (messageTable != null)
                {
                    foreach (DataRow dr in messageTable.Rows)
                    {
                        //留言資料
                        List<Command> commandList = new List<Command>();
                        findSql = String.Format("SELECT USER_ID,USER_NAME,MESSAGE,IMG FROM FORUM" +
                        " WHERE IS_PUBLIC=1 AND UP_ID='" + dr["ID"].ToString().Trim() +
                        "' ORDER BY CRT_DT");
                        DataTable commandTable = GetDataTable(findSql);
                        foreach (DataRow dr2 in commandTable.Rows)
                        {
                            Command command = new Command();
                            command.IMG= GetUserIMG(dr2["USER_ID"].ToString().Trim());
                            command.USER_NAME= dr2["USER_NAME"].ToString().Trim();
                            command.MESSAGE= dr2["MESSAGE"].ToString().Trim();
                            commandList.Add(command);
                        }

                        //討論主題及留言資料
                        Message data = new Message();
                        data.ID = dr["ID"].ToString().Trim();
                        data.USER_ID = dr["USER_ID"].ToString().Trim();
                        data.USER_NAME = dr["USER_NAME"].ToString().Trim();
                        data.USER_IMG = GetUserIMG(dr["USER_ID"].ToString().Trim());
                        data.MESSAGE = dr["MESSAGE"].ToString().Trim();
                        data.IMG = dr["IMG"].ToString().Trim();
                        data.VIDEO = "";//dr["USER_NAME"].ToString();
                        data.UP_ID = 0; //dr["USER_NAME"].ToString();
                        //data.IS_PUBLIC = true;//dr["USER_NAME"].ToString();
                        data.CRT_DT = DateTime.Parse(dr["CRT_DT"].ToString());
                        //data.MDF_DT = null;//dr["USER_NAME"].ToString();
                        data.COMMAND_LIST = commandList;
                        dataList.Add(data);
                    }
                }
            }
            catch (SqlException e)
            {
            }
            return dataList;

        }
        #endregion

        #region 取得使用者照片
        /// <summary>
        /// 取得使用者照片
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public string GetUserIMG(string user_id) 
        {
            string fileSavedPath = WebConfigurationManager.AppSettings["filePath"].ToString();//取得在專案中的路徑
            string publicIMG = fileSavedPath + "public/user.jpg";
            string IMGpath = "";
            string findSql = String.Format("SELECT top 1 USER_IMG FROM USER_INFO" +
            " WHERE IS_USE=1 AND ID="+ user_id +
            " ORDER BY CRT_DT DESC");
            DataTable img = GetDataTable(findSql);
            if (img != null)
            {
                IMGpath = img.Rows[0]["USER_IMG"].ToString().Trim();
            }
            else { IMGpath = publicIMG; }

            return (IMGpath==null|| IMGpath == "") ? publicIMG : IMGpath;
        }
        #endregion

        #region 取得論壇留言(修改用)
        /// <summary>
        /// 取得論壇留言(修改用)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditMessage GetMessage(string id) {
            DataTable result = GetDataTable("SELECT * FROM FORUM WHERE ID = '" + id + "'");
            EditMessage message = new EditMessage();
            if (result != null)
            {
                message.ID = result.Rows[0]["ID"].ToString().Trim();
                message.MESSAGE= result.Rows[0]["MESSAGE"].ToString().Trim();
            }

            return message;
        }
        #endregion

        /// <summary>
        /// 取得參數
        /// </summary>
        /// <param name="type">參數名稱</param>
        /// <returns></returns>
        public List<Parameter> GetParameter(string type)
        {
            DataTable result = GetDataTable("SELECT * FROM PARAMETER WHERE TYPE = '"+ type+"'");
            List<Parameter> strReturn = new List<Parameter>();
            if (result != null)
            {
                foreach (DataRow dr in result.Rows) 
                {
                    Parameter parameter = new Parameter();
                    parameter.VALUE = dr["VALUE"].ToString().Trim();
                    parameter.NAME = dr["VALUE_NAME"].ToString().Trim();
                    strReturn.Add(parameter);
                }
            }


            return strReturn;
        }

    }
}


