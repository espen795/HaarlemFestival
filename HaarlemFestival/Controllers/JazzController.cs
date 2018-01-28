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
            JazzView jazzview = new JazzView();
            jazzview.Jazzs = jazzRepository.GetAllJazzs();
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

        public ActionResult DayOverview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewBag.Id = id;
            int dayId = id.GetValueOrDefault();
            

            List<Jazz> allJazz = jazzRepository.GetAllJazzsByDay(dayId);

            return View(allJazz);
        }


        public ActionResult Reservation()
        {
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
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }


            // In the session
            foreach (BesteldeActiviteit activity in activiteit.Reservering)
            {

                if (activity.Aantal > 0)
                {
                    activity.Activiteit = jazzRepository.GetId(activity.Activiteit.ActivityId);
                   
                    activity.TotalBoughtTickets = activity.Aantal;    
                                           
                    // Prijs berekenen voor in het basket model
                    activity.Price = activity.Aantal * (float)activity.Activiteit.Price;

                    // Prevent ordering more tickets then available
                    orderedActivity.Add(activity);

                }
            }

            Session["current_order"] = orderedActivity;

            // Session op true zetten om de alert te laten zien
            Session["added_to_basket"] = true;

            return RedirectToAction("Reservation", "Jazz");
        }
    }
}