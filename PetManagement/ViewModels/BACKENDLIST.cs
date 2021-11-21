using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetManagement.ViewModels
{
    public class BACKENDLIST
    {
        public class USERINFOLIST
        {
            public UserSearch SearchParameter { get; set; }

            public IPagedList<USERINFO> Users { get; set; }

            public int PageIndex { get; set; }

            public class UserSearch
            {
                public string UserName { get; set; }
                public string Email { get; set; }
                public string Tel { get; set; }
            }
        }
    }
}