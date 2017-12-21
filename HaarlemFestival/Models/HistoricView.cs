using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class HistoricView
    {
        public List<Historic> Tours { get; set; }
        public List<BesteldeActiviteit> Reservering { get; set; }

        public HistoricView(List<Historic> tours)
        {
            Tours = tours;
        }

        public HistoricView()
        {
        }
    }
}