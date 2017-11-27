using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Tour
    {
        public int TourId { get; set; }
        public virtual Languages Language { get; set; }
        public virtual Guides Guide { get; set; }

        public Tour()
        {

        }
    }
}