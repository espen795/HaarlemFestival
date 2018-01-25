using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HaarlemFestival.Models
{
    public class Klant
    {
        public int KlantId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your full name.")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [RegularExpression("(^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$)", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Display(Name = "Payment Method")]
        [Required(ErrorMessage = "Please select a payment method.")]
        public string BetaalMethode { get; set; }
    }
}