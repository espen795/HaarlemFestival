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

            db.SaveChanges();
        }
        public void DeleteEvent(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);

            switch (activity.EventType)
            {
                case EventType.JazzPatronaat:
                    Jazz jazz = db.Jazzs.Find(id);
                    db.Jazzs.Remove(jazz);
                    break;

                case EventType.DinnerInHaarlem:
                    Dinner dinner = db.Dinners.Find(id);
                    db.Dinners.Remove(dinner);
                    break;

                case EventType.TalkingHaarlem:
                    Talking talking = db.Talkings.Find(id);
                    db.Talkings.Remove(talking);
                    break;

                case EventType.HistoricHaarlem:
                    Historic historic = db.Historics.Find(id);
                    db.Historics.Remove(historic);
                    break;
            }

            db.SaveChanges();
        }

        public EventData GetEventData()
        {
            EventData data = new EventData
            {
                Activities = db.Activities.ToList(),

                Dates = db.Days.ToList(),
                Cuisines = db.Cuisines.ToList().GroupBy(c => c.Naam).Select(g => g.First()),
                Languages = db.Languages.ToList(),
                Guides = db.Guides.ToList().GroupBy(g => g.GuideName).Select(g => g.First())
            };

            return data;
        }

       
    }
}