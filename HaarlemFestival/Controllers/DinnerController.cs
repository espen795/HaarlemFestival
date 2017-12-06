using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Reservation()
        {
            return View();
        }
    }
}