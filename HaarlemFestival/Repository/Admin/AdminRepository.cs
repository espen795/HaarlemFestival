using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private HFDB db = new HFDB();

        public Account GetAccount(string username, string password)
        {           
            Account account = db.Accounts.Where(a => (a.Username == username) && (a.Password == password)).FirstOrDefault();
            return account;
        }

        public void AddEvent(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent(Activity activity)
        {
            // TODO: Roep Tabellen aan
            switch(activity.EventType)
            {
                case EventType.JazzPatronaat:
                    break;

                case EventType.DinnerInHaarlem:
                    break;

                case EventType.TalkingHaarlem:
                    break;

                case EventType.HistoricHaarlem:
                    break;
            }
        }

        public void DeleteEvent(Activity activity)
        {
            throw new NotImplementedException();
        }

       
    }
}