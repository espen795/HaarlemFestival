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
        public float KinderPrijs { get; set; }  // NL?
        public int TotalTickets { get; set; }   // EN
        public string Rating { get; set; }
        public string Description { get; set; }
        public int Cuisine1Id { get; set; }
        public virtual Cuisine Cuisine1 { get; set; }
        public int Cuisine2Id { get; set; }
        public virtual Cuisine Cuisine2 { get; set; }
        public int Cuisine3Id { get; set; }
        public virtual Cuisine Cuisine3 { get; set; }
        public string FoodIMG { get; set; }
        public string LocationIMG { get; set; }

        public Restaurant()
        {

        }
    }
}