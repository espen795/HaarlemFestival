using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Admin
{
    public interface IAdminRepository
    {
        // Login
        Account GetAccount(string username, string password);

        // ManageEvent
        void AddEvent(Activity activity);
        void UpdateEvent(Activity activity);
        void DeleteEvent(int id);
        //EventList GetEvents();
    }
}
