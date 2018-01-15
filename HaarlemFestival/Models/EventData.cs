﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class EventData
    {
        public List<Activity> Activities;
        public List<Guide> Guides;
        public List<Restaurant> Restaurants;
        public Filters Filters;
    }
}