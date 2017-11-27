using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Jazz : Activity
    {
        public int JazzId { get; set; }
        public string ConcertLocation { get; set; }
        public string ConcertHall { get; set; }
        public float AllDayPassPartout { get; set; }

        public int ArtistId { get; set; }
        public virtual Artist artist { get; set; }

        public Jazz()
        {

        }
    }
}