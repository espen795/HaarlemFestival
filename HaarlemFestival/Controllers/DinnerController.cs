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

            List<Dinner> DinnersPerRestaurant = dinnerRepository.DinnersPerRestaurant((int)id);

            if (DinnersPerRestaurant.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(DinnersPerRestaurant);
        }

        public ActionResult Reservation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DinnerView dinnerView = new DinnerView();

            dinnerView.Dinners = dinnerRepository.DinnersPerRestaurant((int)id);

            if (dinnerView.Dinners.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Day> days = new List<Day>();
            for (int i = 0; i < dinnerView.Dinners.Count; i++)
            {
                if (i == 0)
                {
                    days.Add(dinnerView.Dinners[i].Day);
                }
                else
                {
                    if (dinnerView.Dinners[i].Day != dinnerView.Dinners[i - 1].Day)
                    {
                        days.Add(dinnerView.Dinners[i].Day);
                    }
                }
            }
            dinnerView.BesteldeActiviteit = new BesteldeActiviteit();
            dinnerView.Days = days;
            return View(dinnerView);
        }

        [HttpPost]
        public ActionResult AddReservation(DinnerView dinnerView, FormCollection collection)
        {
            BesteldeActiviteit besteldeActiviteit = dinnerView.BesteldeActiviteit;
            besteldeActiviteit.Activiteit = dinnerRepository.GetDinnerById(Convert.ToInt32(collection["Activity"]));

            besteldeActiviteit.Price = besteldeActiviteit.Aantal * (float)besteldeActiviteit.Activiteit.Price + besteldeActiviteit.AantalAlternatief * (float)besteldeActiviteit.Activiteit.AlternativePrice;

            int TotalTickets = besteldeActiviteit.Aantal + besteldeActiviteit.AantalAlternatief;

            if(TotalTickets > (besteldeActiviteit.Activiteit.TotalTickets- besteldeActiviteit.Activiteit.BoughtTickets))
            {
                //geef melding uitverkocht
            }

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();

            if (Session["current_order"] != null)
            {
                Bestelling.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            Bestelling.Add(besteldeActiviteit);

            Session["current_order"] = Bestelling;
            return RedirectToAction("Index");
        }
    }
}