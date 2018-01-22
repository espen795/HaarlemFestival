﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Admin
{
    public interface IAdminRepository
    {
        // Account
        List<Account> GetAccounts();
        Account GetAccount(string username, string password);
        Account GetAccountById(int id);
        void AddAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(int id);
        List<Role> GetRoles();

        // ManageEvent
        void AddEvent(Activity activity);
        void AddRestaurant(Restaurant restaurant);
        void AddGuide(Guide guide);

        void UpdateEvent(Activity activity);
        void UpdateRestaurant(Restaurant restaurant);
        void UpdateGuide(Guide guide);

        void DeleteEvent(int id);
        void DeleteRestaurant(int id);
        void DeleteGuide(int id);

        Activity GetActivity(int id);
        Restaurant GetRestaurant(int id);
        Guide GetGuide(int id);
        Cuisine GetCuisine(int id);
        Day GetDay(int id);

        Filters GetFilters();

        EventData GetEventData();
        List<Activity> GetActivities();
        List<Restaurant> GetRestaurants();
        List<Guide> GetGuides();
        List<Cuisine> GetCuisines();
        List<Day> GetDates();
        List<BesteldeActiviteit> GetBesteldeActivities();

        float GetIncomeById(int id);
        float GetIncomeByType(EventType type);

        List<ContactMessage> GetContactMessages();
        ContactMessage GetContactMessage(int id);
        void UpdateContactMessage(ContactMessage message);

        List<InterviewQuestion> GetInterviewQuestions();
    }
}
