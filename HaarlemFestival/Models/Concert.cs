using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Concert
    {
        public int JazzId { get; set; }
        public int RegularTicketAmount { get; set; }
        public int PassPartoutTicketAmount { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Day { get; set; }
        public string Hall { get; set; }

        public Concert()
        {

        }
    }
}