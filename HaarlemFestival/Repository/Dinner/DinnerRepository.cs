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
    }
}