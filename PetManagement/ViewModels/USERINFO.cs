using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class USERINFO
    {
        public int ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string TEL { get; set; }
        public string ADDR { get; set; }
        public string SEX { get; set; }
        public string USER_IMG { get; set; } 
        public System.DateTime CRT_DT { get; set; }
        public Nullable<System.DateTime> MDF_DT { get; set; }
        public bool IS_USE { get; set; }
    }
}