using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public TimeSpan FirstSession { get; set; }
        public double Duration { get; set; }
        public int Sessions { get; set; }
        public int Stars { get; set; }
        public int TotalSeats { get; set; }
        public double Price { get; set; }
        public double ChildrenPrice { get; set; }
        public string Cuisine { get; set; }
    }
}