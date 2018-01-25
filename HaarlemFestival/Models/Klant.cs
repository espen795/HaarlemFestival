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

        [Required(ErrorMessage = "Please enter your full name.")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [RegularExpression("(^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$)", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        public string BetaalMethode { get; set; }
    }
}