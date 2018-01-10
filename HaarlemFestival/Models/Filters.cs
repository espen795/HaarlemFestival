using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Filters
    {
        public List<Restaurant> restaurantFilter;
        public List<Day> days;
        public List<DateModel> dateFilter;
        public List<Guide> guideFilter;
        public List<Cuisine> cuisineFilter;
    }
}