using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class EventData
    {
        public List<Day> Dates;
        public List<Guide> Guides;
        public List<Language> Languages;
        public List<Cuisine> Cuisines;
        public List<Restaurant> Restaurants;

        public List<Activity> Activities;
    }
}