using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class EventData
    {
        public IEnumerable<Day> Dates;
        public IEnumerable<Guide> Guides;
        public IEnumerable<Language> Languages;
        public IEnumerable<Cuisine> Cuisines;
        public IEnumerable<Restaurant> Restaurants;

        public List<Activity> Activities;
    }
}