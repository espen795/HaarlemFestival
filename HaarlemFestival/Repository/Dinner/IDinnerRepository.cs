using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Dinner
{
    interface IDinnerRepository
    {
        List<Restaurant> GetAllRestaurants();
        List<Models.Dinner> DinnersPerRestaurant(int id);
        List<Models.Cuisine> GetAllCuisines();
        Models.Dinner GetDinnerById(int id);
    }
}
