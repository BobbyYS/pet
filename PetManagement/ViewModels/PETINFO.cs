using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class PETINFO
    {
        public int ID { get; set; }
        public int OWNER_ID { get; set; }
        [Required(ErrorMessage = "*暱稱為必填")]
        [Display(Name = "暱稱")]
        public string PET_NAME { get; set; }
        [Required(ErrorMessage = "*寵物種類為必填")]
        [Display(Name = "寵物種類")]
        public string KIND { get; set; }
        [Display(Name = "生日")]
        public DateTime? BIRTHDAY { get; set; }
        [Display(Name = "性別")]
        public string SEX { get; set; }
        public DateTime? CRT_DT { get; set; }
        public DateTime? MDF_DT { get; set; }
        public bool IS_USE { get; set; }
        public string IMG { get; set; }
    }
}