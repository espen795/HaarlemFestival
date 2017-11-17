using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Klant
    {
        public int KlantId { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string BetaalMethode { get; set; }
    }
}