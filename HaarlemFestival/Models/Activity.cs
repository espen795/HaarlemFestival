using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public abstract class Activity
    {
        public int ActivityId { get; set; }
        public EventType EventType { get; set; }
        public DateTime DateReservation { get; set; }
        public DateTime StartSession { get; set; }
        public DateTime EndSession { get; set; }
        public int TotalTickets { get; set; }
        public int BoughtTickets { get; set; }
        public string Phonenumber { get; set; }
        public string Address { get; set; }

        public Activity(EventType eventType, DateTime dateReservation, DateTime startSession, DateTime endSession, int totalTickets, int boughtTickets, string phonenumber, string address)
        {
            EventType = eventType;
            DateReservation = dateReservation;
            StartSession = startSession;
            EndSession = endSession;
            TotalTickets = totalTickets;
            BoughtTickets = boughtTickets;
            Phonenumber = phonenumber;
            Address = address;
        }
    }
}