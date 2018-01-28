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

        //een check voor de algemene pagina of elk restaurant wel een dinner heeft. Zo niet, dan haalt hij hem eruit.
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
            //Voor als het restaurant niet bestaat
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //alle dinners per restaurant ophalen
            List<Dinner> DinnersPerRestaurant = dinnerRepository.DinnersPerRestaurant((int)id);

            //als er geen dinners voor het restaurant bestaan
            if (DinnersPerRestaurant.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(DinnersPerRestaurant);
        }

        public ActionResult Reservation(int? id)
        {
            //als het restaurant niet bestaat
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int Id = (int)id;

            DinnerView dinnerView;

            //voor als de bestelling al eerder mislukt was doordat hij uitverkocht was
            if (TempData["DinnerView"] != null)
                dinnerView = (DinnerView)TempData["DinnerView"];
            else
            {
                dinnerView = FillDinnerView(Id);
                dinnerView.BesteldeActiviteit = new BesteldeActiviteit();
            }

            //Als er geen dinners voor het restaurant zijn
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

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();

            if (Session["current_order"] != null)
            {
                Bestelling.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            //Kijkt of de tickets nog wel beschikbaar zijn
            int inSession = 0;

            foreach (BesteldeActiviteit session in Bestelling)
            {
                if (session.Activiteit.ActivityId == besteldeActiviteit.Activiteit.ActivityId)
                {
                    inSession += session.TotalBoughtTickets;
                }
            }

            if (inSession + besteldeActiviteit.TotalBoughtTickets > (besteldeActiviteit.Activiteit.TotalTickets - besteldeActiviteit.Activiteit.BoughtTickets))
            {
                Session["sold_out"] = true;
                DinnerView view = FillDinnerView(id);
                view.BesteldeActiviteit = new BesteldeActiviteit
                {
                    Aantal = besteldeActiviteit.Aantal,
                    AantalAlternatief = besteldeActiviteit.AantalAlternatief,
                    Opmerking = besteldeActiviteit.Opmerking,
                    Activiteit = besteldeActiviteit.Activiteit
                };
                TempData["DinnerView"] = view;
                return RedirectToAction("Reservation", "Dinner", new { id });
            }

            Bestelling.Add(besteldeActiviteit);

            //Melding voor basket aanzetten
            Session["added_to_basket"] = true;
            
            //Update je bestelling sessie
            Session["current_order"] = Bestelling;

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        //Vullen van Dinnerview voor Reservation
        private DinnerView FillDinnerView(int id)
        {
            DinnerView dinnerView = new DinnerView
            {
                Dinners = dinnerRepository.DinnersPerRestaurant(id)
            };

            dinnerView.Days = new List<Day>();

            foreach (Dinner dinner in dinnerView.Dinners)
            {
                if (!dinnerView.Days.Contains(dinner.Day))
                {
                    dinnerView.Days.Add(dinner.Day);
                }
            }

            return dinnerView;
        }
    }
}