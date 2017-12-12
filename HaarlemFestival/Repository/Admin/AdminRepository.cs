using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaarlemFestival.Models;
using System.Data.Entity;

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
            if (activity.GetType() == typeof(HaarlemFestival.Models.Jazz))
            {
                HaarlemFestival.Models.Jazz jazz = activity as HaarlemFestival.Models.Jazz;
                db.Jazzs.Add(jazz);
            }
            else if (activity.GetType() == typeof(HaarlemFestival.Models.Dinner))
            {
                HaarlemFestival.Models.Dinner dinner = activity as HaarlemFestival.Models.Dinner;
            }
            else if (activity.GetType() == typeof(HaarlemFestival.Models.Talking))
            {
                HaarlemFestival.Models.Talking talking = activity as HaarlemFestival.Models.Talking;
            }
            else if (activity.GetType() == typeof(HaarlemFestival.Models.Historic))
            {
                HaarlemFestival.Models.Historic historic = activity as HaarlemFestival.Models.Historic;
            }

            db.SaveChanges();
        }

        public void UpdateEvent(Activity activity)
        {
            db.Entry(activity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteEvent(int id)
        {
            Activity activity = db.Activities.Find(id);

            switch (activity.EventType)
            {
                case EventType.JazzPatronaat:
                    Models.Jazz jazz = activity as Models.Jazz;
                    db.Artists.Remove(jazz.artist);
                    break;

                case EventType.DinnerInHaarlem:
                    Models.Dinner dinner = activity as Models.Dinner;
                    break;

                case EventType.TalkingHaarlem:
                    Models.Talking talking = activity as Models.Talking;
                    db.Talks.Remove(talking.Talk);
                    db.InterviewQuestions.RemoveRange(db.InterviewQuestions.Where(t => t.InterviewQuestionId == talking.TalkId));
                    break;

                case EventType.HistoricHaarlem:
                    Models.Historic historic = activity as Models.Historic;
                    db.Historics.Remove(historic);
                    break;
            }
            db.Activities.Remove(activity);
            db.SaveChanges();
        }

        public Activity GetActivity(int id)
        {
            return db.Activities.Find(id);
        }

        public EventData GetEventData()
        {
            EventData data = new EventData
            {
                Activities = db.Activities.ToList(),

                Dates = db.Days.ToList(),
                Cuisines = db.Cuisines.ToList(),
                Languages = db.Languages.ToList(),
                Guides = db.Guides.ToList(),
                Restaurants = db.Restaurants.ToList()
            };

            return data;
        }

       
    }
}