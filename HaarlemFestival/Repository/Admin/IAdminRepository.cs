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
        // Login + ManageAccounts
        List<Account> GetAccounts();
        Account GetAccount(string username, string password);
        Account GetAccountById(int id);
        void AddAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(int id);
        List<Role> GetRoles();

        // ManageEvent - Add
        void AddEvent(Activity activity);
        void AddRestaurant(Restaurant restaurant);
        void AddGuide(Guide guide);

        // ManageEvent - Update
        void UpdateEvent(Activity activity);
        void UpdateRestaurant(Restaurant restaurant);
        void UpdateGuide(Guide guide);

        // ManageEvent - Delete
        void DeleteEvent(int id);
        void DeleteRestaurant(int id);
        void DeleteGuide(int id);

        // ManageEvent - Single Data
        Activity GetActivity(int id);
        Restaurant GetRestaurant(int id);
        Guide GetGuide(int id);
        Cuisine GetCuisine(int id);
        Day GetDay(int id);

        // FilterSystem
        Filters GetFilters();

        // ManageEvent - Get All Data
        EventData GetEventData();
        List<Activity> GetActivities();
        List<Restaurant> GetRestaurants();
        List<Guide> GetGuides();
        List<Cuisine> GetCuisines();
        List<Day> GetDates();

        // Ticket & Sales Information
        List<BesteldeActiviteit> GetBesteldeActivities();
        float GetIncomeById(int id);
        float GetIncomeByType(EventType type);

        // AnswerContactMessages
        List<ContactMessage> GetContactMessages();
        ContactMessage GetContactMessage(int id);
        void UpdateContactMessage(ContactMessage message);

        // CheckInterviewQuestions
        List<InterviewQuestion> GetInterviewQuestions();
    }
}
