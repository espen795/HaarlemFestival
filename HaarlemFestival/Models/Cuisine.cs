﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Cuisine
    {
        public int CuisineId { get; set; }
        public string Naam { get; set; }
        public virtual List<Restaurant> Restaurants { get; set; }

        public Cuisine( )
        {

        }
    }
}