using System;
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
        void DeleteEvent(int id);
        void DeleteRestaurant(int id);
        void DeleteGuide(int id);
        void AddRestaurantCuisine(int id);
        Activity GetActivity(int id);
        EventData GetEventData();
    }
}
