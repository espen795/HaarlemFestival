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

        public Cuisine(int cuisineId, string naam)
        {
            CuisineId = cuisineId;
            Naam = naam;
        }
    }
}