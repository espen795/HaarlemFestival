using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HaarlemFestival.Models
{
    public class Language
    {
        [Display(Name = "Language Id")]
        public int LanguageId { get; set; }

        [Display(Name = "Language Name")]
        public string LanguageName { get; set; }

        [Display(Name = "Language Abbreviation")]
        public string LanguageAbbr { get; set; }
    }
}