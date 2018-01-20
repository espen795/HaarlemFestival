using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}