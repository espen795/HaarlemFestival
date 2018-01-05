using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Dinner : Activity
    {
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public string TimeString { get; set; }
        public Dinner()
        {

        }
    }
}