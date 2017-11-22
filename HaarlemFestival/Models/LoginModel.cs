using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = " ")]
        public string Username { get; set; }

        [Required]
        [Display(Name = " ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}