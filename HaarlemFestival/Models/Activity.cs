using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public abstract class Activity
    {
        public int ActivityId { get; set; }
        public DateTime DateReservation { get; set; }
        public DateTime StartSession { get; set; }
        public DateTime EndSession { get; set; }
        public int TotalTickets { get; set; }
        public int BoughtTickets { get; set; }
        public string Phonenumber { get; set; }
        public string Address { get; set; }
    }
}