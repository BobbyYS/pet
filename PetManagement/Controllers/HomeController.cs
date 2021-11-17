﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PetManagement.ViewModels.MESSAGE;
using PetManagement.Models;
using System.Web.Configuration;
using System.IO;
using System.Configuration;

namespace PetManagement.Controllers
{
    public class HomeController : Controller
    {
        Data data = new Data();
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Index()
        {
            List<Message> messageList = data.MessageData();
            return View(messageList);
        }

        public ActionResult _Message(string id,string user_id,string user_img, string name, DateTime time, string message, string img, List<Command> commandList)
        {
            Message messageInf = new Message();
            messageInf.ID = id;
            messageInf.USER_ID = user_id;
            messageInf.USER_NAME = name;
            messageInf.USER_IMG = user_img;
            messageInf.CRT_DT = time;
            messageInf.MESSAGE = message;
            messageInf.IMG = img;
            messageInf.COMMAND_LIST = commandList;
            return PartialView(messageInf);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 新增評論
        /// </summary>
        /// <param name="user">使用者</param>
        /// <param name="comment">評論</param>
        /// <returns></returns>
        public string AddMessage(HttpPostedFileBase file,string id, string user, string comment, bool isPublic = true, int upId = 0)
        {
            bool Success = false;
            string user_IMG="";
            Tuple<bool, string> fileUplodResult = new Tuple<bool, string>(true,"");
            if (file != null) { fileUplodResult = FileUplod(file); }
            if (fileUplodResult.Item1)
            {
                Success = data.ExecChangeData("INSERT INTO FORUM (USER_ID,USER_NAME, MESSAGE,IMG, UP_ID,IS_PUBLIC,CRT_DT)"
                    + " VALUES(" + id + ",'" + user + "','"
                    + comment + "','"+ fileUplodResult.Item2 + "'," + upId.ToString() + ",1,'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "')");

                user_IMG = data.GetUserIMG(id);

            }
            else {
                Response.Write("<script language=javascript>alert(' 檔案上傳失敗，請洽系統管理員 ');</" + "script>");
            }
            return user_IMG;

        }

        #region 刪除評論
        public ActionResult MessageDelete(int Id)
        {
            bool Success = false;
            Success = data.ExecChangeData("DELETE FROM FORUM "
               + " WHERE ID = '" + Id + "' ");
            Success = data.ExecChangeData("DELETE FROM FORUM "
               + " WHERE UP_ID = '" + Id + "' ");

            return Json(Success, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 檔案上傳
        /// </summary>
        /// <param name="file">資料</param>
        /// <returns></returns>
        public Tuple<bool, string> FileUplod(HttpPostedFileBase file)
        {
            string rtnFilePath = "";
            try
            {
                //判斷是否有上傳檔案
                if (Request.Files["file"].ContentLength > 0 )
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileSavedPath = WebConfigurationManager.AppSettings["filePath"].ToString();//取得在專案中的路徑
                    
                    //判斷是否為圖檔
                    if (extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".jpeg")
                    {
                        //更改檔名為當天日期時間
                        string newFileName = string.Concat(
                        DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
                        Path.GetExtension(file.FileName).ToLower());
                        string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath).Replace("Home\\",""), newFileName);

                        // 存放檔案到伺服器上
                        Request.Files["file"].SaveAs(fullFilePath);

                        //寫入要回傳的路徑
                        rtnFilePath = "/"+fileSavedPath+ newFileName;
                        Response.Write("<script language=javascript>alert(' 檔案上傳成功 ');</" +"script>");
                    }
                    else
                    {
                        Response.Write("<script language=javascript>alert('請上傳 .jpg  或 .png 格式的檔案');</" + "script>");
                    }
                }
                return Tuple.Create<bool, string> (true, rtnFilePath); ;
            }
            catch (Exception ex)
            {
                return Tuple.Create<bool, string>(false, rtnFilePath); ;
            }
        }

    }
}