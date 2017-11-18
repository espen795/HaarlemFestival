using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Dinner : Activity
    {
        public EventType EventType = EventType.DinnerInHaarlem;
        public int Id { get; set; }
        public Restaurant Restaurant { get; set; }
        public DateTime DateReservation { get; set; }
        public TimeSpan StartSession { get; set; }
        public TimeSpan EndSession { get; set; }
        public int AvailableSeats { get; set; }
    }
}