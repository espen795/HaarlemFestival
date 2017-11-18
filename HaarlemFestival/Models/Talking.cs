using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Talking : Activity
    {
        public EventType EventType = EventType.TalkingHaarlem;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public InterviewGuest Guest1 { get; set; }
        public InterviewGuest Guest2 { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public string Location { get; set; }
        public string Hall { get; set; }
        public double Price { get; set; }
        public bool ReservationMandatory { get; set; }
        public int MaxTickets { get; set; }
    }
}