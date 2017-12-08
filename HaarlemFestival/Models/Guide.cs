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

        [Display(Name = "Guide Name")]
        public string GuideName { get; set; }

        public Guide()
        {

        }
    }
}