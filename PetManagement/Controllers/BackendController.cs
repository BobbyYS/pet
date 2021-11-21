using PagedList;
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
using static PetManagement.ViewModels.BACKENDLIST;

namespace PetManagement.Controllers
{
    public class BackendController : Controller
    {
        Data data = new Data();
        private int PageSize = 10;

        // GET: Backend
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 檔案上傳
        /// </summary>
        /// <param name="file">資料</param>
        /// <returns></returns>
        public Tuple<bool, string> FileUplod(HttpPostedFileBase file, string replaceName1, string replaceName2)
        {
            string rtnFilePath = "";
            try
            {
                //判斷是否有上傳檔案
                if (Request.Files["FILE_IMG"].ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileSavedPath = WebConfigurationManager.AppSettings["filePath"].ToString();//取得在專案中的路徑
                    string startfileSavedPath = WebConfigurationManager.AppSettings["startPath"].ToString();//取得在專案中的起始路徑

                    //判斷是否為圖檔
                    if (extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".jpeg")
                    {
                        //更改檔名為當天日期時間
                        string newFileName = string.Concat(
                        DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
                        Path.GetExtension(file.FileName).ToLower());
                        string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath).Replace(replaceName1 + "\\", "").Replace(replaceName2 + "\\", ""), newFileName);

                        // 存放檔案到伺服器上
                        Request.Files["FILE_IMG"].SaveAs(fullFilePath);

                        //寫入要回傳的路徑
                        rtnFilePath = startfileSavedPath + fileSavedPath + newFileName;
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

        #region 會員管理
        #region 查詢
        public List<PETINFO> QueryPets()
        {
            DataTable resultPet = data.GetDataTable("SELECT * FROM PET_INFO WHERE IS_USE = '1'");
            List<PETINFO> petInfos = new List<PETINFO>();
            foreach (DataRow rowPet in resultPet.Rows)
            {
                DataTable kind = data.GetDataTable("SELECT * FROM PARAMETER WHERE TYPE = 'PetKind' AND VALUE = '" + rowPet[3].ToString() + "'");
                DataRow dr = kind.Rows[0];
                PETINFO itemPet = new PETINFO
                {
                    ID = Convert.ToInt32(rowPet[0].ToString()),
                    OWNER_ID = Convert.ToInt32(rowPet[1].ToString()),
                    PET_NAME = rowPet[2].ToString(),
                    KIND = dr[3].ToString(),
                    BIRTHDAY = Convert.ToDateTime(rowPet[4].ToString()),
                    SEX = rowPet[5].ToString()
                };
                petInfos.Add(itemPet);
            }
            return petInfos;
        }

        /// <summary>
        /// 會員管理首頁
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult UserManagement(int page = 1)
        {
            List<USERINFO> userInfos = new List<USERINFO>();
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE IS_USE = '1'");
            foreach (DataRow row in result.Rows)
            {

                USERINFO item = new USERINFO
                {
                    ID = Convert.ToInt32(row[0].ToString()),
                    USER_NAME = row[1].ToString().Trim(),
                    EMAIL = row[3].ToString().Trim(),
                    TEL = row[5].ToString().Trim()
                };
                userInfos.Add(item);
            }

            ViewBag.List2 = QueryPets();

            var query = userInfos.AsQueryable();

            int pageIndex = page < 1 ? 1 : page;

            var model = new USERINFOLIST
            {
                SearchParameter = new USERINFOLIST.UserSearch(),
                PageIndex = pageIndex,
                Users = query.ToPagedList(pageIndex, PageSize)
            };

            return View(model);
        }

        /// <summary>
        /// 會員管理查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserManagement(USERINFOLIST model)
        {
            List<USERINFO> userInfos = new List<USERINFO>();
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE IS_USE = '1'");
            foreach (DataRow row in result.Rows)
            {
                USERINFO item = new USERINFO
                {
                    ID = Convert.ToInt32(row[0].ToString()),
                    USER_NAME = row[1].ToString().Trim(),
                    EMAIL = row[3].ToString().Trim(),
                    TEL = row[5].ToString().Trim()
                };
                userInfos.Add(item);
            }

            ViewBag.List2 = QueryPets();

            var query = userInfos.AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchParameter.UserName))
            {
                query = query.Where(
                    x => x.USER_NAME.Contains(model.SearchParameter.UserName.Trim())).OrderBy(x => x.ID);
            }
            if (!string.IsNullOrEmpty(model.SearchParameter.Email))
            {
                query = query.Where(
                    x => x.EMAIL.Contains(model.SearchParameter.Email.Trim())).OrderBy(x => x.ID);
            }
            if (!string.IsNullOrEmpty(model.SearchParameter.Tel))
            {
                query = query.Where(
                    x => x.TEL == model.SearchParameter.Tel.Trim()).OrderBy(x => x.ID);
            }

            int pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;

            var resultList = new USERINFOLIST
            {
                SearchParameter = model.SearchParameter,
                PageIndex = model.PageIndex < 1 ? 1 : model.PageIndex,
                Users = query.ToPagedList(pageIndex, PageSize)
            };

            return View(resultList);
        }
        #endregion

        #region 主人
        #region 編輯
        /// <summary>
        /// 會員管理編輯
        /// </summary>
        /// <returns></returns>
        public ActionResult UserModify(int ID)
        {
            DataTable result = data.GetDataTable("SELECT * FROM USER_INFO WHERE ID = '" + ID + "'");
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
        public ActionResult UserModify(int ID, USERINFO userInfo)
        {
            bool Success = false;

            Tuple<bool, string> fileUplodResult = new Tuple<bool, string>(true, "");
            if (userInfo.FILE_IMG != null) { fileUplodResult = FileUplod(userInfo.FILE_IMG, "Backend", "UserModify"); }
            if (fileUplodResult.Item1)
            {
                Success = data.ExecChangeData("UPDATE USER_INFO SET "
                + " USER_NAME = '" + userInfo.USER_NAME + "', "
                + " TEL = '" + userInfo.TEL + "', "
                + " ADDR = '" + userInfo.ADDR + "', "
                + " SEX = '" + userInfo.SEX + "', "
                + " USER_IMG = '" + fileUplodResult.Item2 + "', "
                + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
                + " WHERE ID = '" + ID + "' ");
            }

            TempData["OwnerModify"] = Success;
            return PartialView();
        }
        #endregion

        #region 刪除
        /// <summary>
        /// 會員刪除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult UserDelete(int ID)
        {
            bool Success = false;
            Success = data.ExecChangeData("UPDATE USER_INFO SET "
               + " IS_USE = '0', "
               + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
               + " WHERE ID = '" + ID + "' ");

            return Json(Success, JsonRequestBehavior.AllowGet);
        }
        #endregion
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
        #region 編輯
        /// <summary>
        /// 會員管理編輯
        /// </summary>
        /// <returns></returns>
        public ActionResult PetModify(int ID)
        {
            ViewBag.DDLKIND = DDLPetType();
            DataTable result = data.GetDataTable("SELECT * FROM PET_INFO WHERE ID = '" + ID + "'");
            DataRow dr = result.Rows[0];

            PETINFO petInfo = new PETINFO
            {
                PET_NAME = dr[2].ToString().Trim(),
                KIND = dr[3].ToString(),
                BIRTHDAY = Convert.ToDateTime(dr[4]),
                SEX = dr[5].ToString(),
                IMG = dr[9].ToString()
            };
            return PartialView(petInfo);
        }

        [HttpPost]
        public ActionResult PetModify(int ID, PETINFO petInfo)
        {
            ViewBag.DDLKIND = DDLPetType();
            bool Success = false;
            Tuple<bool, string> fileUplodResult = new Tuple<bool, string>(true, "");
            if (petInfo.FILE_IMG != null) { fileUplodResult = FileUplod(petInfo.FILE_IMG, "Backend", "PetModify"); }
            if (fileUplodResult.Item1)
            {
                Success = data.ExecChangeData("UPDATE PET_INFO SET "
               + " PET_NAME = '" + petInfo.PET_NAME + "', "
               + " KIND = '" + petInfo.KIND + "', "
               + " BIRTHDAY = '" + petInfo.BIRTHDAY.Value.ToString("yyyy/MM/dd HH:mm:ss") + "', "
               + " SEX = '" + petInfo.SEX + "', "
               + " IMG = '" + fileUplodResult.Item2 + "', "
               + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
               + " WHERE ID = '" + ID + "' ");
            }
            TempData["PetModify"] = Success;
            return PartialView();
        }
        #endregion

        #region 刪除
        /// <summary>
        /// 會員刪除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult PetDelete(int ID)
        {
            bool Success = false;
            Success = data.ExecChangeData("UPDATE PET_INFO SET "
               + " IS_USE = '0', "
               + " MDF_DT = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' "
               + " WHERE ID = '" + ID + "' ");

            return Json(Success, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #endregion
    }
}