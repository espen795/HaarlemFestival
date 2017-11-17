using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Reserveringen
    {
        public int ReserveringId { get; set; }
        public virtual Klant Klant { get; set; }
        public List<BesteldeActiviteit>BesteldeActiviteiten { get; set; }
        public float Prijs { get; set; }
    }
}