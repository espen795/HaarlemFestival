using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class CuisinesPerRestaurant
    {
        public int CuisinesPerRestaurantId { get; set; }
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public int CuisineId { get; set; }
        public virtual Cuisine Cuisine { get; set; }
    }
}