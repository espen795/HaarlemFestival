using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Jazz
    {
        public int Id { get; set; }
        public string Band { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
        public string Hall { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public double Price { get; set; }
    }
}