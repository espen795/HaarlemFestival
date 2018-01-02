using System;
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
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Basket()
        {
            List<BesteldeActiviteit> act = (List<BesteldeActiviteit>)Session["current_order"];

            ViewBag.Message = "Your shopping basket.";

            return View(act);
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