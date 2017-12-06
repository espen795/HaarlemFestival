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

        [Display(Name = "Price")]
        public float? Price { get; set; }

        [Display(Name = "Alternative Price")]
        public float? AlternativePrice { get; set; }

        [Display(Name = "Total Tickets")]
        public int TotalTickets { get; set; }

        [Display(Name = "Bought Tickets")]
        public int BoughtTickets { get; set; }

        [Display(Name = "Phone number")]
        public string Phonenumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        //Altijd 1 lege constructor houden voor de database!
        public Activity()
        {

        }
    }
}