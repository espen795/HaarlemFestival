using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HaarlemFestival.Models
{
    public class Talk
    {
        [Display(Name = "Talk Id")]
        public int TalkId { get; set; }

        [Display(Name = "Name")]
        public string Naam { get; set; }

        [Display(Name = "Person 1")]
        public string Person1 { get; set; }

        [Display(Name = "Person 2")]
        public string Person2 { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Maximum Tickets")]
        public int MaxTickets { get; set; }

        [Display(Name = "Image Person 1")]
        public string Person1IMG { get; set; }

        [Display(Name = "Image Person 1 - Alternative")]
        public string Person1AltIMG { get; set; }

        [Display(Name = "Image Person 2")]
        public string Person2IMG { get; set; }

        [Display(Name = "Image Person 2 - Alternative")]
        public string Person2AltIMG { get; set; }

        public Talk()
        {

        }
    }
}