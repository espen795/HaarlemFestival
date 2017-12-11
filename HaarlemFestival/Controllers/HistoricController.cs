using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using HaarlemFestival.Repository.Historic;

namespace HaarlemFestival.Controllers
{
    public class HistoricController : Controller
    {
        private IHistoricRepository historicRepository = new HistoricRepository();

        public ActionResult Index()
        {
            List<Location> allLocations = historicRepository.GetAllLocations();

            return View(allLocations);
        }

        public ActionResult Reservation()
        {
            List<Historic> allTours = historicRepository.GetAllTours();

            return View(allTours);
        }
    }
}