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
            HistoricView toursview = new HistoricView();
            toursview.Tours = historicRepository.GetAllTours();
            toursview.Reservering = new List<BesteldeActiviteit>();

            return View(toursview);
        }

        [HttpPost]
        public ActionResult Book(HistoricView activiteit)
        {
            List<BesteldeActiviteit> orderedActivity = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            int i = 0;
            // In the session
            foreach (BesteldeActiviteit activity in activiteit.Reservering)
            {
                if (activity.Aantal != 0 || activity.AantalAlternatief != 0)
                {
                    // Totaal aantal tickets berekenen (alternatief telt voor 4 plekken in een tour)
                    activity.TotalBoughtTickets = activity.Aantal + (activity.AantalAlternatief * 4);

                    // Tour ophalen gebaseerd op het id
                    activity.Activiteit = historicRepository.GetTourForId(activity.Activiteit.ActivityId);

                    // Prijs berekenen voor in het basket model
                    activity.Price = activity.Aantal * (float)activity.Activiteit.Price + activity.AantalAlternatief * (float)activity.Activiteit.AlternativePrice;

                    orderedActivity.Add(activity);
                }
            }

            Session["current_order"] = orderedActivity;

            // Partial view continue or basket
            return RedirectToAction("Reservation", "Historic");
        }
    }
}