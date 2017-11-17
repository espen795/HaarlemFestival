using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Historic
    {
        public int Id { get; set; }
        public DateTime DateReservation { get; set; }
        public TimeSpan TimeReservation { get; set; }
        public string StartLocation { get; set; }
        public Guide Guide { get; set; }
        public Language Language { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public double Price { get; set; }
        public double GroupPrice { get; set; }
        public bool ReservationMandatory { get; set; }
    }
}