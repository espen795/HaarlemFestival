using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Scripts
{
    interface IAdminRepository
    {
        // Login
        Account GetAccount();

        // Manage Events
        void UpdateEvent();
        void DeleteEvent();
        EventList GetEvents();
    }
}
