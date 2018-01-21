using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class ManageAccountViewModel
    {
        public Account Account { get; set; }
        public List<Role> Roles { get; set; }
    }
}