using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using HaarlemFestival.Repository.Jazz;
using System.Net;

namespace HaarlemFestival.Controllers
{
    public class JazzController : Controller
    {
        private IJazzRepository jazzRepository = new JazzRepository();

        public ActionResult Index()
        {
            List<Jazz> allJazz = jazzRepository.GetAllJazzs();     
            return View(allJazz);
        }

        public ActionResult DayOverview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } else
            {
                ViewBag.Id = id;
            }

            List<Jazz> allJazz = jazzRepository.GetAllJazzs();

            return View(allJazz);
        }


        public ActionResult Reservation()
        {
            JazzView jazzview = new JazzView();
            jazzview.Jazzs = jazzRepository.GetAllJazzs();
            jazzview.Reservering = new List<BesteldeActiviteit>();

            return View(jazzview);
        }

        [HttpPost]
        public ActionResult Book(JazzView activiteit)
        {
            List<BesteldeActiviteit> orderedActivity = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }


            // In the session
            foreach (BesteldeActiviteit activity in activiteit.Reservering)
            {
                if (activity.Aantal != 0)
                {
                    // Totaal aantal tickets berekenen (alternatief telt voor 4 plekken in een tour)
                    activity.TotalBoughtTickets = activity.Aantal;
           
                    activity.Activiteit = jazzRepository.GetId(activity.Activiteit.ActivityId);

                    // Prijs berekenen voor in het basket model
                    activity.Price = activity.Aantal * (float)activity.Activiteit.Price;

                    orderedActivity.Add(activity);
                }
            }

            Session["current_order"] = orderedActivity;

            // Partial view continue or basket
            Session["added_to_basket"] = true;

            return RedirectToAction("Reservation", "Jazz");
        }
    }
}