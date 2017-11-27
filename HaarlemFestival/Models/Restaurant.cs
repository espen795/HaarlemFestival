using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Naam { get; set; }
        public float Prijs { get; set; }
        public float KinderPrijs { get; set; }
        public int TotalTickets { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public List<Cuisine> Cuisines { get; set; }
        public string FoodIMG { get; set; }
        public string LocationIMG { get; set; }

        public Restaurant()
        {

        }
    }
}