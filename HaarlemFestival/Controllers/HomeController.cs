using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using HaarlemFestival.Repository.Jazz;
using HaarlemFestival.Repository.Dinner;
using HaarlemFestival.Repository.Talking;
using HaarlemFestival.Repository.Historic;

namespace HaarlemFestival.Controllers
{
    public class HomeController : Controller
    {
        private IJazzRepository jazzRepository = new JazzRepository();
        private IDinnerRepository dinnerRepository = new DinnerRepository();
        private ITalkingRepository talkingRepository = new TalkingRepository();
        private IHistoricRepository historicRepository = new HistoricRepository();

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
            BasketModel basketModel = new BasketModel();

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();
            Bestelling = (List<BesteldeActiviteit>)Session["current_order"];

            basketModel.Order = Bestelling;
            basketModel.Jazzs = jazzRepository.GetAllJazzs();
            basketModel.Dinners = dinnerRepository.GetAllDinners();
            basketModel.Talks = talkingRepository.GetAllTalks();
            //basketModel.Historics = historicRepository.GetAllTours()

            
            return View(basketModel);
        }

        public ActionResult RemoveFromBasket(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int Id = (int)id;

            List<BesteldeActiviteit> Bestelling = (List<BesteldeActiviteit>)Session["current_order"];

            foreach(BesteldeActiviteit besteldeActiviteit in Bestelling)
            {
                if(besteldeActiviteit.Activiteit.ActivityId == Id)
                {
                    Bestelling.Remove(besteldeActiviteit);
                    break;
                }
            }

            Session["current_order"] = Bestelling;

            return View("Basket", "Home");
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