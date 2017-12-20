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
            List<Jazz> allJazz = jazzRepository.GetAllJazzs();

            return View(allJazz);
        }
    }
}