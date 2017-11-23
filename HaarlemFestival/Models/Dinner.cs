using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Dinner : Activity
    {
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public Dinner(EventType eventType, DateTime dateReservation, DateTime startSession, DateTime endSession, int totalTickets, int boughtTickets, string phonenumber, string address, int restaurantid, Restaurant restaurant) : 
            base(eventType, dateReservation, startSession, endSession, totalTickets, boughtTickets, phonenumber, address)
        {
            RestaurantId = restaurantid;
            Restaurant = restaurant;
        }
    }
}