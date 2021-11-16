using PetManagement.App_Code;
using PetManagement.Models;
using PetManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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

            DataTable petResult = data.GetDataTable("SELECT * FROM PET_INFO WHERE OWNER_ID = '" + userId + "' AND IS_USE = '1' ");
            List<PETINFO> petInfos = new List<PETINFO>();
            foreach (DataRow row in petResult.Rows)
            {
                petInfos.Add(new PETINFO
                {
                    ID = Convert.ToInt32(row[0]),
                    PET_NAME = row[2].ToString().Trim()
                });
            }
            userinfo.petinfoList = petInfos;
            return View(userinfo);
        }

        #region 主人
        public ActionResult Owner()
        {
            string userId = Session["userId"] as string;
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE ID = '" + userId + "'");
            DataRow dr = result.Rows[0];

            USERINFO userinfo = new USERINFO
            {
                USER_NAME = dr[1].ToString().Trim(),
                EMAIL = dr[3].ToString().Trim(),
                USER_IMG = dr[4].ToString().Trim(),
                TEL = dr[5].ToString().Trim(),
                ADDR = dr[6].ToString().Trim(),
                SEX = dr[7].ToString().Trim()
            };
            return PartialView(userinfo);
        }

        [HttpPost]
        public ActionResult Owner(USERINFO userInfo)
        {
            bool Success = false;
            string userId = Session["userId"] as string;

            Success = data.ExecChangeData("UPDATE USER_INFO SET "
                + " USER_NAME = '" + userInfo.USER_NAME + "', "
                + " TEL = '" + userInfo.TEL + "', "
                + " ADDR = '" + userInfo.ADDR + "', "
                + " SEX = '" + userInfo.SEX + "', "
                + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
                + " WHERE ID = '" + userId + "' ");

            if (Success)
            {
                Session["userName"] = userInfo.USER_NAME;
            }

            TempData["OwnerModify"] = Success;
            return PartialView();
        }
        #endregion

        #region 寵物 
        /// <summary>
        /// 下拉選單(寵物種類)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> DDLPetType()
        {
            DataTable result = data.GetDataTable("SELECT * FROM PARAMETER WHERE TYPE = 'PetKind' ");
            List<SelectListItem> oblist = new List<SelectListItem>();

            foreach (DataRow row in result.Rows)
            {
                SelectListItem ob = new SelectListItem
                {
                    Value = row[2].ToString().Trim(),
                    Text = row[3].ToString().Trim()
                };
                oblist.Add(ob);
            }
            return oblist;
        }

        #region 新增寵物
        public ActionResult PetCreate()
        {
            ViewBag.DDLKIND = DDLPetType();
            return PartialView();
        }

        [HttpPost]
        public ActionResult PetCreate(PETINFO petInfo)
        {
            bool Success = false;
            string userId = Session["userId"] as string;
            ViewBag.DDLKIND = DDLPetType();
            Success = data.ExecChangeData("INSERT INTO PET_INFO (OWNER_ID,PET_NAME,KIND,BIRTHDAY,SEX,CRT_DT,IS_USE)"
                        + " VALUES('" + userId + "','" + petInfo.PET_NAME + "','"
                        + petInfo.KIND + "','" + petInfo.BIRTHDAY.Value.ToString("yyyy/MM/dd HH:mm:ss") + "','" + petInfo.SEX + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','1')");

            TempData["PetCreate"] = Success;
            return PartialView();
        }
        #endregion

        #region 編輯寵物
        public ActionResult PetModify(int Id)
        {
            ViewBag.DDLKIND = DDLPetType();
            DataTable result = data.GetDataTable("SELECT * FROM PET_INFO WHERE ID = '" + Id + "' ");
            DataRow dr = result.Rows[0];
            PETINFO petInfo = new PETINFO
            {
                PET_NAME = dr[2].ToString().Trim(),
                KIND = dr[3].ToString(),
                BIRTHDAY = Convert.ToDateTime(dr[4]),
                SEX = dr[5].ToString()
            };

            return PartialView(petInfo);
        }

        [HttpPost]
        public ActionResult PetModify(int Id, PETINFO petInfo)
        {
            ViewBag.DDLKIND = DDLPetType();
            bool Success = false;
            Success = data.ExecChangeData("UPDATE PET_INFO SET "
               + " PET_NAME = '" + petInfo.PET_NAME + "', "
               + " KIND = '" + petInfo.KIND + "', "
               + " BIRTHDAY = '" + petInfo.BIRTHDAY.Value.ToString("yyyy/MM/dd HH:mm:ss") + "', "
               + " SEX = '" + petInfo.SEX + "', "
               + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
               + " WHERE ID = '" + Id + "' ");

            TempData["PetModify"] = Success;
            return PartialView();
        }
        #endregion

        #region 刪除寵物
        public ActionResult PetDelete(int Id)
        {
            bool Success = false;
            Success = data.ExecChangeData("UPDATE PET_INFO SET "
               + " IS_USE = '0', "
               + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
               + " WHERE ID = '" + Id + "' ");

            return Json(Success, JsonRequestBehavior.AllowGet);
        }
        #endregion

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
                if (Request.Files["file"].ContentLength > 0)
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
                        string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath).Replace("Home\\", ""), newFileName);

                        // 存放檔案到伺服器上
                        Request.Files["file"].SaveAs(fullFilePath);

                        //寫入要回傳的路徑
                        rtnFilePath = "/" + fileSavedPath + newFileName;
                        Response.Write("<script language=javascript>alert(' 檔案上傳成功 ');</" + "script>");
                    }
                    else
                    {
                        Response.Write("<script language=javascript>alert('請上傳 .jpg  或 .png 格式的檔案');</" + "script>");
                    }
                }
                return Tuple.Create<bool, string>(true, rtnFilePath);
            }
            catch (Exception ex)
            {
                return Tuple.Create<bool, string>(false, rtnFilePath);
            }
        }

    }
}