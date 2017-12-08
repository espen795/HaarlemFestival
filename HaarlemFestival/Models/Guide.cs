using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class Guide
    {
        [Display(Name = "Guide Id")]
        public int GuideId { get; set; }

        [Required(ErrorMessage = "Please enter the name of the guide.")]
        [Display(Name = "Guide Name")]
        public string GuideName { get; set; }
        public DateTime WorksOn { get; set; }

        public Guide()
        {

        }
    }
}