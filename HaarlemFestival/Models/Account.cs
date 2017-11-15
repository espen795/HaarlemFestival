using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}