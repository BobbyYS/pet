using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class MESSAGE
    {
        public class Message 
        {
            public string USER_NAME { get; set; }
            public string MESSAGE { get; set; }
            public string IMG { get; set; }
            public string VIDEO { get; set; }
            public int UP_ID { get; set; }
            public bool IS_PUBLIC { get; set; }
            public DateTime CRT_DT { get; set; }
            public DateTime MDF_DT { get; set; }
        }
    }
}