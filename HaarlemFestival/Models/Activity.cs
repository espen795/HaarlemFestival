using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public abstract class Activity
    {
        public int ActivityId { get; set; }
        public EventType EventType { get; set; }

        [Display(Name = "Start Session")]
        public DateTime StartSession { get; set; }

        [Display(Name = "End Sesion")]
        public DateTime EndSession { get; set; }

        [Required(ErrorMessage = "Please enter the price of the event.")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [RegularExpression("([0-9]*[.,]\\d\\d*)", ErrorMessage = "Please fill in a valid price")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public float? Price { get; set; }

        [Display(Name = "Alternative Price")]
        [DataType(DataType.Currency)]
        [RegularExpression("([0-9]*[.,]\\d\\d*)", ErrorMessage = "Please fill in a valid price")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public float? AlternativePrice { get; set; }

        [Required(ErrorMessage = "Please enter the total amount of tickets.")]
        [Display(Name = "Total Tickets")]
        public int TotalTickets { get; set; }

        [Display(Name = "Bought Tickets")]
        public int BoughtTickets { get; set; }

        [Required(ErrorMessage = "Please enter a phone number.")]
        [Display(Name = "Phone number")]
        public string Phonenumber { get; set; }

        [Required(ErrorMessage = "Please enter an address.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //Altijd 1 lege constructor houden voor de database!
        public Activity()
        {

        }
    }
}