using PetManagement.App_Code;
using PetManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetManagement.ViewModels;

namespace PetManagement.Controllers
{
    public class LoginController : Controller
    {
        Data data = new Data();
        Fun_Public pub = new Fun_Public();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(USERINFO userInfo)
        {
            string errorMessage = string.Empty;
            bool Success = false;
            //判斷是否已註冊過
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE EMAIL = '" + userInfo.EMAIL + "' AND IS_USE = '1' ");
            if (result.Rows.Count > 0)
            {
                errorMessage = "此Email已註冊過，請直接登入!";
            }
            else
            {
                //新增
                userInfo.PASSWORD = pub.SHA512(userInfo.PASSWORD);

                Success = data.ExecChangeData("INSERT INTO USER_INFO (USER_NAME,PASSWORD,EMAIL,TEL,ADDR,SEX,CRT_DT,IS_USE)"
                        + " VALUES('" + userInfo.USER_NAME + "','" + userInfo.PASSWORD + "','"
                        + userInfo.EMAIL + "','" + userInfo.TEL + "','" + userInfo.ADDR + "','" + userInfo.SEX + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','1')");

                if (!Success)
                {
                    errorMessage = "請洽系統管理員！";
                }
            }

            var json = new { status = Success, message = errorMessage };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(USERINFO userInfo)
        {
            string errorMessage = string.Empty;
            bool Success = false;
            userInfo.PASSWORD = pub.SHA512(userInfo.PASSWORD);
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE EMAIL = '" + userInfo.EMAIL + "' AND PASSWORD = '" + userInfo.PASSWORD + "' AND IS_USE = '1' ");
            if (result.Rows.Count == 0)
            {
                errorMessage = "密碼或Email錯誤!";
            }
            else
            {
                Success = true;
                DataRow dr = result.Rows[0];
                Session["userId"] = dr[0].ToString().Trim();
                Session["userName"] = dr[1].ToString().Trim();
            }

            var json = new { status = Success, message = errorMessage };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reset(USERINFO userInfo)
        {
            string errorMessage = string.Empty;
            bool Success = false;
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE EMAIL = '" + userInfo.EMAIL + "'");

            if (result.Rows.Count == 0)
            {
                errorMessage = "此帳號尚未註冊!";
            }
            else
            {
                //新增
                userInfo.PASSWORD = pub.SHA512(userInfo.PASSWORD);

                Success = data.ExecChangeData("UPDATE USER_INFO SET PASSWORD = '" + userInfo.PASSWORD + "' WHERE EMAIL = '" + userInfo.EMAIL + "'");

                if (!Success)
                {
                    errorMessage = "請洽系統管理員！";
                }
            }

            var json = new { status = Success, message = errorMessage };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult GetUserName()
        {
            string userName = Session["userName"] as string;
            ViewBag.UserName = userName;

            return PartialView("_GetUserName");
        }

        public ActionResult Logout()
        {
            Session.Contents.RemoveAll();
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}