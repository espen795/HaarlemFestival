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
            // Getting all available tours
            HistoricView toursview = new HistoricView();
            toursview.Tours = historicRepository.GetAllTours();
            toursview.Reservering = new List<BesteldeActiviteit>();

            // List with days
            toursview.Days = new List<Day>();

            // Adding the days
            foreach (Historic tour in toursview.Tours)
            {
                if (!toursview.Days.Contains(tour.Day))
                {
                    toursview.Days.Add(tour.Day);
                }
            }

            return View(toursview);
        }

        [HttpPost]
        public ActionResult Book(HistoricView activiteit)
        {
            List<BesteldeActiviteit> orderedActivity = new List<BesteldeActiviteit>();
            int inSession = 0;

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }


            // In the session
            foreach (BesteldeActiviteit activity in activiteit.Reservering)
            {
                // Getting bought tickets from session
                foreach (BesteldeActiviteit session in orderedActivity)
                {
                    if (session.Activiteit.ActivityId == activity.Activiteit.ActivityId)
                    {
                        inSession += session.TotalBoughtTickets;
                    }
                }

                // Prevent ordering negative amount
                if (activity.Aantal > 0 || activity.AantalAlternatief > 0)
                {
                    // Get tour based on id
                    activity.Activiteit = historicRepository.GetTourForId(activity.Activiteit.ActivityId);

                    // Calculate total ordered tickets (alternative ticket counts as 4)
                    activity.TotalBoughtTickets = activity.Aantal + (activity.AantalAlternatief * 4);

                    // Calculating price for the basket
                    activity.Price = activity.Aantal * (float)activity.Activiteit.Price + activity.AantalAlternatief * (float)activity.Activiteit.AlternativePrice;

                    // Prevent ordering more tickets then available
                    if (inSession + activity.TotalBoughtTickets <= (activity.Activiteit.TotalTickets - activity.Activiteit.BoughtTickets))
                    {
                        orderedActivity.Add(activity);
                    }
                }
            }

            Session["current_order"] = orderedActivity;

            // Partial view continue or basket
            Session["added_to_basket"] = true;

            // Partial view continue or basket
            return RedirectToAction("Reservation", "Historic");
        }
    }
}