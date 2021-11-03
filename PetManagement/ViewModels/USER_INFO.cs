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
        public string USER_NAME { get; set; }
        [Required(ErrorMessage = "*密碼為必填")]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage = "*信箱為必填")]
        [EmailAddress]
        public string EMAIL { get; set; }
        public string TEL { get; set; }
        public string ADDR { get; set; }
        public string SEX { get; set; }
        public System.DateTime CRT_DT { get; set; }
        public Nullable<System.DateTime> MDF_DT { get; set; }
        public bool IS_USE { get; set; }
    }
}