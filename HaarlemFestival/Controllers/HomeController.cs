﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;

namespace HaarlemFestival.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendContactForm()
        {
            return PartialView("_ContactSent");
        }

        public ActionResult Basket()
        {
            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();
            Bestelling = (List<BesteldeActiviteit>)Session["current_order"];
            
            return View(Bestelling);
        }

        public ActionResult Agenda()
        {
            ViewBag.Message = "Your personal agenda.";

            return View();
        }

        public ActionResult Reservation()
        {
            ViewBag.Message = "Your reservation.";

            return View();
        }
    }
}