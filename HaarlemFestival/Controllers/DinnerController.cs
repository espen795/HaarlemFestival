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
            Cuisines.Add(new Cuisine
            {
                Naam = "[Select an option]",
                Restaurants = dinnerRepository.GetAllRestaurants()
            });
            Cuisines.AddRange(dinnerRepository.GetAllCuisines());

            OnlyGetUsedRestaurants(Cuisines);

            return View(Cuisines);
        }

        private void OnlyGetUsedRestaurants(List<Cuisine> Cuisines)
        {
            List<Dinner> Dinners = dinnerRepository.GetAllDinners();

            List<int> UsedRestaurantIds = new List<int>();

            List<int> AllRestaurantIds = new List<int>();

            foreach (Restaurant restaurant in Cuisines[0].Restaurants)
            {
                AllRestaurantIds.Add(restaurant.RestaurantId);
            }

            foreach (Dinner dinner in Dinners)
            {
                if (AllRestaurantIds.Contains(dinner.Restaurant.RestaurantId))
                {
                    AllRestaurantIds.Remove(dinner.Restaurant.RestaurantId);
                }
            }

            foreach (int Id in AllRestaurantIds)
            {
                for (int i = 0; i < Cuisines.Count; i++)
                {
                    Cuisines[i].Restaurants.RemoveAll(r => r.RestaurantId == Id);
                }
            }
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

            int Id = (int)id;

            DinnerView dinnerView;


            if (TempData["DinnerView"] != null)
                dinnerView = (DinnerView)TempData["DinnerView"];
            else
            {
                dinnerView = FillDinnerView(Id);
                dinnerView.BesteldeActiviteit = new BesteldeActiviteit();
            }

            if (dinnerView.Dinners.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(dinnerView);
        }

        [HttpPost]
        public ActionResult AddReservation(DinnerView dinnerView, FormCollection collection, int id)
        {
            BesteldeActiviteit besteldeActiviteit = dinnerView.BesteldeActiviteit;
            besteldeActiviteit.Activiteit = dinnerRepository.GetDinnerById(Convert.ToInt32(collection["Activity"]));

            besteldeActiviteit.Price = besteldeActiviteit.Aantal * (float)besteldeActiviteit.Activiteit.Price + besteldeActiviteit.AantalAlternatief * (float)besteldeActiviteit.Activiteit.AlternativePrice;

            besteldeActiviteit.TotalBoughtTickets = besteldeActiviteit.Aantal + besteldeActiviteit.AantalAlternatief;

            if (besteldeActiviteit.TotalBoughtTickets > (besteldeActiviteit.Activiteit.TotalTickets - besteldeActiviteit.Activiteit.BoughtTickets))
            {
                Session["sold_out"] = true;
                DinnerView view = FillDinnerView(id);
                view.BesteldeActiviteit = new BesteldeActiviteit();
                view.BesteldeActiviteit.Aantal = besteldeActiviteit.Aantal;
                view.BesteldeActiviteit.AantalAlternatief = besteldeActiviteit.AantalAlternatief;
                view.BesteldeActiviteit.Opmerking = besteldeActiviteit.Opmerking;
                view.BesteldeActiviteit.Activiteit = besteldeActiviteit.Activiteit;
                TempData["DinnerView"] = view;
                return RedirectToAction("Reservation", "Dinner", new { id });
            }

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();

            if (Session["current_order"] != null)
            {
                Bestelling.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            Bestelling.Add(besteldeActiviteit);

            Session["added_to_basket"] = true;

            Session["current_order"] = Bestelling;
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        private DinnerView FillDinnerView(int id)
        {
            DinnerView dinnerView = new DinnerView
            {
                Dinners = dinnerRepository.DinnersPerRestaurant(id)
            };

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
            dinnerView.Days = days;

            return dinnerView;
        }
    }
}