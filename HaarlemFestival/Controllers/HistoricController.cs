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
            HistoricView historicView = historicRepository.GetAllTours();

            return View(historicView);
        }

        [HttpPost]
        public ActionResult Book(HistoricView activiteit)
        {
            List<BesteldeActiviteit> b = new List<BesteldeActiviteit>();
            // Session existing?
            if (Session["current_order"] != null)
            {
                b.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }
      
            // In the session
            foreach (BesteldeActiviteit act in activiteit.Reservering)
            {
                if(act.Aantal != 0 || act.AantalAlternatief != 0)
                {
                    act.Activiteit = historicRepository.GetTourForId(act.Activiteit.ActivityId);
                    b.Add(act);
                }
            }

            Session["current_order"] = b;
            // Partial view continue or basket
            return RedirectToAction("Reservation", "Historic");
        }
    }
}