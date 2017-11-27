using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Tour
    {
        public int TourId { get; set; }
        public int TotalTickets { get; set; }
        public Languages Language { get; set; }
        public Guides Guide { get; set; }

        public Tour()
        {

        }
    }
}