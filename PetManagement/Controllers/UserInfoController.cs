using PetManagement.Models;
using PetManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetManagement.Controllers
{
    public class UserInfoController : Controller
    {
        Data data = new Data();

        // GET: UserInfo
        public ActionResult Index()
        {
            string userName = Session["userName"] as string;
            string userId = Session["userId"] as string;
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE ID = '" + userId + "'");

            DataRow dr = result.Rows[0];
            string url = dr[4].ToString().Trim();

            USERINFO userinfo = new USERINFO
            {
                USER_NAME = userName,
                USER_IMG = url
            };
            return View(userinfo);
        }
    }
}