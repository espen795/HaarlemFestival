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
        void AddReservation(Reservering reservation);
        void ChangeTickets(Activity activity);
    }
}
