using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetManagement.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Park()
        {
            return View();
        }
        public ActionResult _Park()
        {
            return View();
        }
        public ActionResult Salon()
        {
            return View();
        }
        public ActionResult Hospital()
        {
            return View();
        }
    }
}