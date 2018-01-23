using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class AgendaView2
    {
        public List<BesteldeActiviteit> Activities { get; set; }
        public List<Day> Days { get; set; }
        public List<Jazz> Jazzs { get; set; }
        public List<Dinner> Dinners { get; set; }
        public List<Talking> Talks { get; set; }
        public List<Historic> Historics { get; set; }
    }
}