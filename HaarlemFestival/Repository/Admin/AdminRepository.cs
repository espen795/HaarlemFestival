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
            if (activity.GetType() == typeof(Models.Jazz))
            {
                Models.Jazz jazz = activity as Models.Jazz;
                db.Jazzs.Add(jazz);
            }
            else if (activity.GetType() == typeof(Models.Dinner))
            {
                Models.Dinner dinner = activity as Models.Dinner;
                db.Dinners.Add(dinner);
            }
            else if (activity.GetType() == typeof(Models.Talking))
            {
                Models.Talking talking = activity as Models.Talking;
                db.Talkings.Add(talking);
            }
            else if (activity.GetType() == typeof(Models.Historic))
            {
                Models.Historic historic = activity as Models.Historic;
                db.Historics.Add(historic);
            }

            db.SaveChanges();
        }

        public void AddRestaurant(Restaurant restaurant)
        {
            db.Restaurants.Add(restaurant);
            db.SaveChanges();
        }

        public void AddGuide(Guide guide)
        {
            db.Guides.Add(guide);
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

        public void AddRestaurantCuisine(int id)
        {
            
        }

        public void DeleteRestaurant(int id)
        {
            db.Dinners.RemoveRange(db.Dinners.Where(d => d.RestaurantId == id));
            db.Restaurants.RemoveRange(db.Restaurants.Where(r => r.RestaurantId == id));
            db.SaveChanges();
        }

        public void DeleteGuide(int id)
        {
            Guide guide = db.Guides.Find(id);
            db.Historics.RemoveRange(db.Historics.Where(h => h.GuideId == guide.GuideId));
            db.Guides.Remove(guide);
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
                Guides = db.Guides.ToList(),
                Restaurants = db.Restaurants.ToList()
            };

            return data;
        }

       
    }
}