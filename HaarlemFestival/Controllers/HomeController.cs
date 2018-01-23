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
using HaarlemFestival.Repository.Admin;

namespace HaarlemFestival.Controllers
{
    public class HomeController : Controller
    {
        private IJazzRepository jazzRepository = new JazzRepository();
        private IDinnerRepository dinnerRepository = new DinnerRepository();
        private ITalkingRepository talkingRepository = new TalkingRepository();
        private IHistoricRepository historicRepository = new HistoricRepository();
        private IAdminRepository adminRepository = new AdminRepository();
        private IHomeRepository homerepository = new HomeRepository();

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
            ContactMessage message = new ContactMessage();
            return View(message);
        }

        [HttpPost]
        public ActionResult Contact(ContactMessage message, FormCollection collector)
        {
            int regardingint;
            if (int.TryParse(collector["RegardingSelect"], out regardingint))
            {
                message.Regarding = (QuestionCategory)regardingint;
            }
            else
            {
                ModelState.AddModelError("RegardingError", "Please select a category.");
            }


            if (ModelState.IsValid)
            {

                Session["send_contact_form"] = true;

                homerepository.SendContactMessage(message);
            }

            return View(message);
        }

        public ActionResult Basket()
        {
            BasketModel basketModel = new BasketModel
            {
                Jazzs = jazzRepository.GetAllJazzs(),
                Dinners = dinnerRepository.GetAllDinners(),
                Talks = talkingRepository.GetAllTalks(),
                Historics = historicRepository.GetAllTours()
            };

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();
            if (Session["current_order"] != null)
                Bestelling.AddRange((List<BesteldeActiviteit>)Session["current_order"]);

            List<BesteldeActiviteit> Order = new List<BesteldeActiviteit>();
            if (Bestelling.Count > 0)
            {
                Bestelling = Bestelling.OrderBy(x => x.Activiteit.ActivityId).ToList();


                Order.Add(Bestelling[0]);
                for (int i = 1; i < Bestelling.Count; i++)
                {
                    if (Bestelling[i].Activiteit.ActivityId == Bestelling[i - 1].Activiteit.ActivityId)
                    {
                        BesteldeActiviteit besteldeActiviteit = Order.Where(x => x.Activiteit.ActivityId == Bestelling[i].Activiteit.ActivityId).FirstOrDefault();
                        Order.RemoveAll(x => x.Activiteit.ActivityId == Bestelling[i].Activiteit.ActivityId);
                        besteldeActiviteit.Aantal += Bestelling[i].Aantal;
                        besteldeActiviteit.AantalAlternatief += Bestelling[i].AantalAlternatief;
                        besteldeActiviteit.TotalBoughtTickets += Bestelling[i].TotalBoughtTickets;
                        besteldeActiviteit.Price += Bestelling[i].Price;
                        besteldeActiviteit.Opmerking += " \n " + Bestelling[i].Opmerking;
                        Order.Add(besteldeActiviteit);
                    }
                    else
                        Order.Add(Bestelling[i]);
                }

            }

            basketModel.Order = Order;

            return View(basketModel);
        }

        public ActionResult RemoveFromBasketById(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int Id = (int)id;

            List<BesteldeActiviteit> Bestelling = (List<BesteldeActiviteit>)Session["current_order"];

            Bestelling.RemoveAll(x => x.Activiteit.ActivityId == Id);
            Session["current_order"] = Bestelling;

            BasketModel basketModel = new BasketModel
            {
                Order = Bestelling,
                Jazzs = jazzRepository.GetAllJazzs(),
                Dinners = dinnerRepository.GetAllDinners(),
                Talks = talkingRepository.GetAllTalks(),
                Historics = historicRepository.GetAllTours()
            };

            return View("Basket", basketModel);
        }

        public ActionResult Agenda()
        {
            AgendaView agendaView = new AgendaView
            {
                Day1 = new List<BesteldeActiviteit>(),
                Day2 = new List<BesteldeActiviteit>(),
                Day3 = new List<BesteldeActiviteit>(),
                Day4 = new List<BesteldeActiviteit>()
            };

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();

            if ((List<BesteldeActiviteit>)Session["current_order"] != null)
            {
                Bestelling = (List<BesteldeActiviteit>)Session["current_order"];
            }

            if (Bestelling.Count != 0)
            {
                agendaView.Day1.AddRange(Bestelling.Where(b => b.Activiteit.Day.DayId == 1));
                agendaView.Day2.AddRange(Bestelling.Where(b => b.Activiteit.Day.DayId == 2));
                agendaView.Day3.AddRange(Bestelling.Where(b => b.Activiteit.Day.DayId == 3));
                agendaView.Day4.AddRange(Bestelling.Where(b => b.Activiteit.Day.DayId == 4));
            }

            agendaView.Jazzs = jazzRepository.GetAllJazzs();
            agendaView.Dinners = dinnerRepository.GetAllDinners();
            agendaView.Talks = talkingRepository.GetAllTalks();
            agendaView.Historics = historicRepository.GetAllTours();

            return View(agendaView);
        }

        public ActionResult Reservation()
        {
            return View(new Reservering());
        }

        [HttpPost]
        public ActionResult MakeReservation(Reservering reservation)
        {
            homerepository.AddKlant(reservation.Klant);

            reservation = homerepository.AddReservation(reservation);

            reservation.BesteldeActiviteiten = new List<BesteldeActiviteit>();

            if ((List<BesteldeActiviteit>)Session["current_order"] != null)
            {
                reservation.BesteldeActiviteiten = (List<BesteldeActiviteit>)Session["current_order"];
            }

            foreach (BesteldeActiviteit besteldeactiviteit in reservation.BesteldeActiviteiten)
            {
                besteldeactiviteit.Reservering_ReserveringId = reservation.ReserveringId;
                                
                besteldeactiviteit.Activiteit.BoughtTickets += besteldeactiviteit.TotalBoughtTickets;
                
                homerepository.ChangeTickets(besteldeactiviteit);
            }
           
            return RedirectToAction("Completed");
        }

        public ActionResult Completed()
        {
            ViewBag.Message = "Thanks for booking.";

            return View();
        }
    }
}