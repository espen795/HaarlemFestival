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
            // Jazzview (ViewModel) aanmaken en vullen
            JazzView jazzview = new JazzView();
            jazzview.Jazzs = jazzRepository.GetAllJazzs();
            jazzview.Days = new List<Day>();

            // Alle Jazzs langsgaan en de dag in de lijst zetten indien deze er nog niet is
            foreach (Jazz jazz in jazzview.Jazzs)
            {
                if (!jazzview.Days.Contains(jazz.Day))
                {
                    jazzview.Days.Add(jazz.Day);
                }
            }

            return View(jazzview);
        }

        public ActionResult DayOverview(int? id)
        {
            // Als er geen ID in de URL staat BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            // DayId vullen met het ID uit de URL
            int dayId = id.GetValueOrDefault();
           
            // Lust AllJazz vullen met alle events van het ID
            List<Jazz> allJazz = jazzRepository.GetAllJazzsByDay(dayId);

            return View(allJazz);
        }


        public ActionResult Reservation()
        {
            // Jazzview (ViewModel) aanmaken en vullen
            JazzView jazzview = new JazzView();
            jazzview.Jazzs = jazzRepository.GetAllJazzs();
            jazzview.Reservering = new List<BesteldeActiviteit>();
            jazzview.Days = new List<Day>();

            foreach (Jazz jazz in jazzview.Jazzs)
            {
                if (!jazzview.Days.Contains(jazz.Day))
                {
                    jazzview.Days.Add(jazz.Day);
                }
            }

            return View(jazzview);
        }

        [HttpPost]
        public ActionResult Book(JazzView activiteit)
        {
            List<BesteldeActiviteit> orderedActivity = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                // Indien sessie al bestaat de lijst vullen met wat er in de sessie zit
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }


            // In the session
            foreach (BesteldeActiviteit activity in activiteit.Reservering)
            {
                // Kijken of het aantal ingevoerde tickets wel een positief getal is.
                if (activity.Aantal > 0)
                {
                    // Jazz event ophalen aan de hand van het ID uit activity
                    // Dit ID wordt meegestuurd, staat in een hidden field
                    activity.Activiteit = jazzRepository.GetId(activity.Activiteit.ActivityId);
                   
                    activity.TotalBoughtTickets = activity.Aantal;    
                                           
                    // Prijs berekenen voor in het basket model
                    activity.Price = activity.Aantal * (float)activity.Activiteit.Price;

                    // Event toevoegen aan de lijst
                    orderedActivity.Add(activity);

                }
            }

            // De lijst in de sessie plaatsen
            Session["current_order"] = orderedActivity;

            // Session op true zetten om de alert te laten zien
            Session["added_to_basket"] = true;

            return RedirectToAction("Reservation", "Jazz");
        }
    }
}