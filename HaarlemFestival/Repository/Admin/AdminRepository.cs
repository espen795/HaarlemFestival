﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaarlemFestival.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace HaarlemFestival.Repository.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private HFDB db = new HFDB();

        #region Login + ManageAccounts
        public List<Account> GetAccounts()
        {
            return db.Accounts.ToList();
        }

        public Account GetAccount(string username, string password)
        {
            Account account = db.Accounts.Where(a => (a.Username == username) && (a.Password == password)).FirstOrDefault();
            return account;
        }

        public Account GetAccountById(int id)
        {
            return db.Accounts.Find(id);
        }

        public void AddAccount(Account account)
        {
            db.Accounts.Add(account);
            db.SaveChanges();
        }

        public void UpdateAccount(Account account)
        {
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteAccount(int id)
        {
            Account account = GetAccountById(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
        }
        
        public List<Role> GetRoles()
        {
            return db.Roles.ToList();
        }
        #endregion

        #region ManageEvent - Add
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
        #endregion

        #region ManageEvent - Update
        public void UpdateEvent(Activity activity)
        {
            db.Entry(activity).State = EntityState.Modified;

            switch (activity.EventType)
            {
                case EventType.TalkingHaarlem:
                    Models.Talking talk = activity as Models.Talking;
                    db.Entry(talk.Talk).State = EntityState.Modified;
                    break;
            }

            db.SaveChanges();
        }

        public void UpdateRestaurant(Restaurant restaurant)
        {
            db.Entry(restaurant).State = EntityState.Modified;

            db.Database.ExecuteSqlCommand("DELETE FROM CuisineRestaurants WHERE Restaurant_RestaurantId = {0}", restaurant.RestaurantId);
            foreach (Cuisine cuisine in restaurant.Cuisines)
                UpdateRestaurantCuisine(restaurant.RestaurantId, cuisine.CuisineId);

            db.SaveChanges();
        }

        public void UpdateRestaurantCuisine(int restaurantId, int cuisineId)
        {
            db.Database.ExecuteSqlCommand("INSERT INTO CuisineRestaurants (Cuisine_CuisineId, Restaurant_RestaurantId) values ({0}, {1})", cuisineId, restaurantId);
            db.SaveChanges();
        }

        public void UpdateGuide(Guide guide)
        {
            db.Entry(guide).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region ManageEvent - Delete
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
        #endregion

        #region ManageEvent - Get Single Data
        public Activity GetActivity(int id)
        {
            return db.Activities.Find(id);
        }

        public Restaurant GetRestaurant(int id)
        {
            return db.Restaurants.Find(id);
        }

        public Guide GetGuide(int id)
        {
            return db.Guides.Find(id);
        }

        public Cuisine GetCuisine(int id)
        {
            return db.Cuisines.Where(c => c.CuisineId == id).FirstOrDefault();
        }

        public Day GetDay(int id)
        {
            return db.Days.Where(d => d.DayId == id).FirstOrDefault();
        }
        #endregion

        #region FilterSystem
        public Filters GetFilters()
        {
            Filters filters = new Filters
            {
                restaurantFilter = db.Restaurants.ToList(),
                guideFilter = db.Guides.ToList(),
                days = db.Days.ToList(),
                cuisineFilter = db.Cuisines.ToList()
            };

            return filters;
        }
        #endregion

        #region ManageEvent - Get All Data
        public EventData GetEventData()
        {
            EventData data = new EventData
            {
                Activities = db.Activities.ToList(),
                Restaurants = db.Restaurants.ToList(),
                Guides = db.Guides.ToList(),
                Filters = GetFilters()
            };

            return data;
        }

        public List<Activity> GetActivities()
        {
            return db.Activities.ToList();
        }

        public List<Restaurant> GetRestaurants()
        {
            return db.Restaurants.ToList();
        }

        public List<Guide> GetGuides()
        {
            return db.Guides.ToList();
        }

        public List<Cuisine> GetCuisines()
        {
            return db.Cuisines.ToList();
        }

        public List<Day> GetDates()
        {
            return db.Days.ToList();
        }
        #endregion

        #region Ticket & Sales Information
        public List<BesteldeActiviteit> GetBesteldeActivities()
        {
            return db.BesteldeActiviteiten.ToList();
        }

        public float GetIncomeById(int activityId)
        {
            List<BesteldeActiviteit> activities = db.BesteldeActiviteiten.Where(b => b.Activiteit.ActivityId == activityId).ToList();
            return activities.Sum(a => a.Price);
        }

        public float GetIncomeByType(EventType type)
        {
            List<BesteldeActiviteit> activities = db.BesteldeActiviteiten.Where(b => b.Activiteit.EventType == type).ToList();
            return activities.Sum(a => a.Price);
        }
        #endregion

        #region AnswerContactMessages
        public List<ContactMessage> GetContactMessages()
        {
            return db.ContactMessages.ToList();
        }

        public ContactMessage GetContactMessage(int id)
        {
            return db.ContactMessages.Find(id);
        }

        public void UpdateContactMessage(ContactMessage message)
        {
            db.Entry(message).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region CheckInterviewQuestions
        public List<InterviewQuestion> GetInterviewQuestions()
        {
            return db.InterviewQuestions.Where(q => q.Content != null && q.Receiver != null).ToList();
        }
        #endregion
    }
}