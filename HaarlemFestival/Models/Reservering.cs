using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Reservering
    {
        public int ReserveringId { get; set; }
        public int KlantId { get; set; }
        public virtual Klant Klant { get; set; }
        public List<BesteldeActiviteit>BesteldeActiviteiten { get; set; }
        public float TotaalPrijs { get; set; }
    }
}