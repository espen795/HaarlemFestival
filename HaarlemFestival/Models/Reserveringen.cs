using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Reserveringen
    {
        public int Id { get; set; }
        public int Tickets { get; set; }
        public virtual Event Event { get; set; }
        
        public virtual Days Day { get; set; }
    }
}