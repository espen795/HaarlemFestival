using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }

        public string ArtistImage { get; set; }
        public string ArtistInformation { get; set; }


        public Artist()
        {

        }
    }
}