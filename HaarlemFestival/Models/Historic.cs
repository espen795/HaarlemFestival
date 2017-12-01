using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Historic : Activity
    {
        public int TourId { get; set; }

        public virtual Tour Tour { get; set; }

        public Historic()
        {

        }
    }
}