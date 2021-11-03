using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class USER_INFO
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "*姓名為必填")]
        [Display(Name = "姓名")]
        public string USER_NAME { get; set; }
        [Required(ErrorMessage = "*密碼為必填")]
        [Display(Name = "密碼")]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage = "*信箱為必填")]
        [Display(Name = "信箱")]
        public string EMAIL { get; set; }
        [Display(Name = "電話")]
        public string TEL { get; set; }
        [Display(Name = "地址")]
        public string ADDR { get; set; }
        [Display(Name = "性別")]
        public string SEX { get; set; }
        public System.DateTime CRT_DT { get; set; }
        public Nullable<System.DateTime> MDF_DT { get; set; }
        public bool IS_USE { get; set; }
    }
}