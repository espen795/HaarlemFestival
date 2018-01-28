using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class JazzView
    {
        public List<Jazz> Jazzs { get; set; }
        public List<Day> Days { get; set; }
        public List<BesteldeActiviteit> Reservering { get; set; }

        public JazzView(List<Jazz> jazzs)
        {
            Jazzs = jazzs;
        }

        public JazzView()
        {
        }
    }
}