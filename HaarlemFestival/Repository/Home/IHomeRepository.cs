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
        void ChangeTickets(Models.Historic activity);
        void SendContactMessage(ContactMessage message);
        void AddBesteldeActiviteiten(List<BesteldeActiviteit> besteldeActiviteiten);
    }
}
