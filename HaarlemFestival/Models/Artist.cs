using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class Artist
    {
        
        public int ArtistId { get; set; }

        [Display(Name = "Name")]
        public string ArtistName { get; set; }

        [Display(Name = "Image")]
        public string ArtistImage { get; set; }

        [Display(Name = "Description")]
        public string ArtistInformation { get; set; }


        public Artist()
        {

        }
    }
}