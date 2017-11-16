using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Admin
{
    public class AdminRepository : IAdminRepository
    {
        public Account GetAccount(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent()
        {
            throw new NotImplementedException();
        }

        public void DeleteEvent()
        {
            throw new NotImplementedException();
        }

        public EventList GetEvents()
        {
            throw new NotImplementedException();
        }
    }
}