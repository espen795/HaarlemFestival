using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Dinner
{
    public class DinnerRepository : IDinnerRepository
    {
        private HFDB db = new HFDB();
        public List<Restaurant> GetAllRestaurants()
        {
            List<Restaurant> restaurants = db.Restaurants.ToList();

            return restaurants;
        }

        public List<Models.Dinner> DinnersPerRestaurant( int id)
        {
            List<Models.Dinner> Dinners = new List<Models.Dinner>();
            Dinners.AddRange(db.Dinners.Where(d => d.RestaurantId == id).ToList());

            return Dinners;
        }

        public List<Models.Cuisine> GetAllCuisines()
        {
            List<Models.Cuisine> cuisines = db.Cuisines.ToList();

            return cuisines;
        }

        public Models.Dinner GetDinnerById(int id)
        {
            return db.Dinners.Where(d => d.ActivityId == id).FirstOrDefault();
        }
    }
}