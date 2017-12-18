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
        // Login
        Account GetAccount(string username, string password);

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

        EventData GetEventData();
        List<Restaurant> GetRestaurants();
        List<Guide> GetGuides();
        List<Cuisine> GetCuisines();
        List<Day> GetDates();

    }
}
