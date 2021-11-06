using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PetManagement.ViewModels.MESSAGE;
using PetManagement.Models;

namespace PetManagement.Controllers
{
    public class HomeController : Controller
    {
        Data data = new Data();
        public ActionResult Index()
        {
            //insert 範例
            //bool a = data.ExecChangeData("INSERT INTO FORUM (USER_NAME, MESSAGE, UP_ID,IS_PUBLIC,CRT_DT)"
            //    + " VALUES('陳OO', 'testInsert', 0,1,'" + DateTime.Now.ToString("yyyy/MM/dd") + "')");
            List<Message> aa= data.MessageData();
            return View();
        }

        public ActionResult _Message()
        {
            ViewBag.Message = "Your application description page.";

            return PartialView();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}