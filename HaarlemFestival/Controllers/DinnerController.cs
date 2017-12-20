using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using HaarlemFestival.Repository.Dinner;

namespace HaarlemFestival.Controllers
{
    public class DinnerController : Controller
    {
        private IDinnerRepository dinnerRepository = new DinnerRepository();
        public ActionResult Index()
        {
            List<Restaurant> allRestaurants = dinnerRepository.GetAllRestaurants();

            return View(allRestaurants);
        }

        public ActionResult Index2()
        {
            List<Cuisine> Cuisines = new List<Cuisine>();
            Cuisine c = new Cuisine
            {
                Naam = "[Select an option]",
                Restaurants = dinnerRepository.GetAllRestaurants()
            };
            Cuisines.Add(c);
            Cuisines.AddRange(dinnerRepository.GetAllCuisines());

            return View(Cuisines);
        }

        public ActionResult Info(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Dinner> OptionsPerRestaurant = dinnerRepository.DinnersPerRestaurant((int)id);

            return View(OptionsPerRestaurant);
        }

        public ActionResult Reservation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Dinner> OptionsPerRestaurant = dinnerRepository.DinnersPerRestaurant((int)id);

            return View(OptionsPerRestaurant);
        }
    }
}