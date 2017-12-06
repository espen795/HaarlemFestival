using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HaarlemFestival.Models
{
    public class Restaurant
    {
        [Display(Name = "Restaurant Id")]
        public int RestaurantId { get; set; }

        [Display(Name = "Name")]
        public string Naam { get; set; }

        [Display(Name = "Rating")]
        public string Rating { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Food Image")]
        public string FoodIMG { get; set; }

        [Display(Name = "Location Image")]
        public string LocationIMG { get; set; }
        public virtual List<Cuisine> Cuisines { get; set; }
        public Restaurant()
        {

        }
    }
}