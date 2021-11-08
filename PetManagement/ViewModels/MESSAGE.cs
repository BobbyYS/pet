using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class MESSAGE
    {
        public class Message 
        {
            public string USER_ID { get; set; }
            public string USER_NAME { get; set; }
            public string MESSAGE { get; set; }
            public string IMG { get; set; }
            public string VIDEO { get; set; }
            public int UP_ID { get; set; }
            public bool IS_PUBLIC { get; set; }
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm:ss}")]
            public DateTime CRT_DT { get; set; }
            public DateTime MDF_DT { get; set; }
            public List<Command> COMMAND_LIST { get; set; }
        }

        public class Command {
            public string IMG { get; set; }
            public string USER_NAME { get; set; }
            public string MESSAGE { get; set; }

        }
    }
}