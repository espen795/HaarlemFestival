using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Dinner
{
    public class HomeRepository : IHomeRepository
    {
        private HFDB db = new HFDB();

        public void AddKlant(Klant klant)
        {
            db.Klanten.Add(klant);
            db.SaveChanges();
        }

        public Reservering AddReservation(Reservering reservation)
        {
            db.Reserveringen.Add(reservation);
            db.SaveChanges();

            return reservation;
        }

        public void ChangeTickets(BesteldeActiviteit besteldeActiviteit)
        {
            Activity dbActivity = db.Activities.Find(besteldeActiviteit.Activiteit.ActivityId);
            dbActivity.BoughtTickets += besteldeActiviteit.TotalBoughtTickets;
            db.Entry(dbActivity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void ChangeTicketsJazz(Activity activity, BesteldeActiviteit besteldeActiviteit)
        {
            Activity dbActivity = db.Activities.Find(activity.ActivityId);
            dbActivity.BoughtTickets += besteldeActiviteit.TotalBoughtTickets;
            db.Entry(dbActivity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void SendContactMessage(ContactMessage message)
        {
            db.ContactMessages.Add(message);
            db.SaveChanges();
        }

        public void AddBesteldeActiviteit(BesteldeActiviteit besteldeActiviteit)
        {
            Activity dbActivity = db.Activities.Find(besteldeActiviteit.Activiteit.ActivityId);

            besteldeActiviteit.Activiteit = dbActivity;

            db.BesteldeActiviteiten.Add(besteldeActiviteit);
            db.SaveChanges();
        }
    }
}