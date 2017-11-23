using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class BesteldeActiviteit
    {
        public virtual Reserveringen Reservering{ get; set; }
        public int Aantal { get; set; }
        public string Opmerking { get; set; }
        public virtual Activity activiteit { get; set; }
    }
}