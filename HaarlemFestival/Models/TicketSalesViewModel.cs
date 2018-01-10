using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class TicketSalesViewModel
    {
        public List<Activity> Activities;
        public List<BesteldeActiviteit> BesteldeActiviteiten;
        public Filters Filters;
    }
}