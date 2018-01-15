using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class AgendaView
    {
        public List<BesteldeActiviteit> Day1 { get; set; }
        public List<BesteldeActiviteit> Day2 { get; set; }
        public List<BesteldeActiviteit> Day3 { get; set; }
        public List<BesteldeActiviteit> Day4 { get; set; }
        public List<Jazz> Jazzs { get; set; }
        public List<Dinner> Dinners { get; set; }
        public List<Talking> Talks { get; set; }
        public List<Historic> Historics { get; set; }
    }
}