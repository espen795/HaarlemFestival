﻿using System;
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
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                id = 0;
            }

            List<Cuisine> Cuisines = new List<Cuisine>();
            Cuisine c = new Cuisine
            {
                Naam = "[Select an option]",
                Restaurants = dinnerRepository.GetAllRestaurants()
            };
            Cuisines.Add(c);
            Cuisines.AddRange(dinnerRepository.GetAllCuisines());
            ViewBag.CuisineId = id;
            return View(Cuisines);
        }

        public ActionResult Info(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Dinner> OptionsPerRestaurant = dinnerRepository.DinnersPerRestaurant((int)id);

            return View(OptionsPerRestaurant);
        }

        public ActionResult Reservation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DinnerView dinnerView = new DinnerView();

            dinnerView.Dinners = dinnerRepository.DinnersPerRestaurant((int)id);
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
            dinnerView.DayString = dinnerView.Dinners[0].Day.Naam;
            dinnerView.DinnersOnDay = new List<Dinner>();
            return View(dinnerView);
        }
    }
}