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

        [Required(ErrorMessage = "Please enter the guide's name.")]
        [Display(Name = "Guide Name")]
        public string GuideName { get; set; }

        [Display(Name = "Language Name")]
        public string LanguageName { get; set; }

        [Display(Name = "Language Abbreviation")]
        public string LanguageAbbr { get; set; }

        public Guide()
        {

        }
    }
}