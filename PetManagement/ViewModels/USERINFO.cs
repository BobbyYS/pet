using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class USERINFO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "*姓名為必填")]
        [Display(Name = "姓名")]
        public string USER_NAME { get; set; }
        public string PASSWORD { get; set; }
        [Display(Name = "EMAIL")]
        public string EMAIL { get; set; }
        [Display(Name = "電話")]
        public string TEL { get; set; }
        [Display(Name = "地址")]
        public string ADDR { get; set; }
        [Display(Name = "性別")]
        public string SEX { get; set; }
        public string USER_IMG { get; set; }
        public System.DateTime CRT_DT { get; set; }
        public Nullable<System.DateTime> MDF_DT { get; set; }
        public bool IS_USE { get; set; }
        public List<PETINFO> petinfoList { get; set; }
    }
}