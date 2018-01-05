using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class DinnerView
    {
        public List<Dinner> Dinners { get; set; }
        public BesteldeActiviteit BesteldeActiviteit { get; set; }
        public List<Day> Days { get; set; }
        public List<Dinner> DinnersOnDay { get; set; }
        public string DayString { get; set; }
        public string TimeString { get; set; }
    }
}