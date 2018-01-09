using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class Jazz : Activity
    {
        [Required]
        [Display(Name = "Concert Location")]
        public string ConcertLocation { get; set; }

        [Display(Name = "Concert Hall")]
        public string ConcertHall { get; set; }

        [Display(Name = "Artist ID")]
        public int? ArtistId { get; set; }
        public virtual Artist artist { get; set; }

        public Jazz()
        {

        }
    }
}