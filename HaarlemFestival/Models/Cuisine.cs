using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HaarlemFestival.Models
{
    public class Cuisine
    {
        [Display(Name = "Cuisine Id")]
        public int CuisineId { get; set; }

        [Display(Name = "Cuisine Naam")]
        public string Naam { get; set; }
        public virtual List<Restaurant> Restaurants { get; set; }

        public Cuisine( )
        {

        }
    }
}