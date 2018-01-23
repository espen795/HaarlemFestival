using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Dinner
{
    interface IHomeRepository
    {
        void AddKlant(Klant klant);
        Reservering AddReservation(Reservering reservation);
        void ChangeTickets(BesteldeActiviteit activity);
        void ChangeTicketsJazz(Activity activity, BesteldeActiviteit besteldeActiviteit);
        void SendContactMessage(ContactMessage message);
        void AddBesteldeActiviteit(BesteldeActiviteit besteldeActiviteit);
    }
}
