//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FORUM
    {
        public int ID { get; set; }
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string MESSAGE { get; set; }
        public string IMG { get; set; }
        public string VIDEO { get; set; }
        public int UP_ID { get; set; }
        public bool IS_PUBLIC { get; set; }
        public System.DateTime CRT_DT { get; set; }
        public Nullable<System.DateTime> MDF_DT { get; set; }
    }
}
