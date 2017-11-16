using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Admin
{
    interface IAdminRepository
    {
        // Login
        Account GetAccount();

        // ManageEvent
        void UpdateEvent();
        void DeleteEvent();
        EventList GetEvents();
    }
}
