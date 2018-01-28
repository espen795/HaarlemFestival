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

        public ActionResult Contact()
        {
            ContactMessage message = new ContactMessage();
            return View(message);
        }

        [HttpPost]
        public ActionResult Contact(ContactMessage message, FormCollection collector)
        {
            //regarding vullen (int word los aangemaakt, omdat die het in de tryparse niet werkte)
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
            //basket model vullen voor info in de basket en cross-selling
            BasketModel basketModel = new BasketModel
            {
                Jazzs = jazzRepository.GetAllJazzs(),
                Dinners = dinnerRepository.GetAllDinners(),
                Talks = talkingRepository.GetAllTalks(),
                Historics = historicRepository.GetAllTours()
            };

            //bestelling ophalen
            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();
            if (Session["current_order"] != null)
                Bestelling.AddRange((List<BesteldeActiviteit>)Session["current_order"]);

            //Kijken of een bestelde activiteit 2 keer voorkomt in de bestelling
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
                Session["current_order"] = Order;
            }

            basketModel.Order = Order;

            return View(basketModel);
        }

        public ActionResult RemoveFromBasketById(int? id)
        {
            //als het activiteit id niet bestaat
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int Id = (int)id;

            List<BesteldeActiviteit> Bestelling = (List<BesteldeActiviteit>)Session["current_order"];

            //verwijderen van de activiteit
            Bestelling.RemoveAll(x => x.Activiteit.ActivityId == Id);
            Session["current_order"] = Bestelling;

            //basketmodel opnieuw vullen voor de basket 
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
            //agenda view voor bestellingen laden 
            AgendaView agendaView = new AgendaView
            {
                Jazzs = jazzRepository.GetAllJazzs(),
                Dinners = dinnerRepository.GetAllDinners(),
                Talks = talkingRepository.GetAllTalks(),
                Historics = historicRepository.GetAllTours()
            };

            List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();

            if ((List<BesteldeActiviteit>)Session["current_order"] != null)
            {
                Bestelling = (List<BesteldeActiviteit>)Session["current_order"];
                Session["current_order"] = null;
            }

            agendaView.Days = new List<Day>();

            foreach (BesteldeActiviteit besteldeActiviteit in Bestelling)
            {
                if (!agendaView.Days.Contains(besteldeActiviteit.Activiteit.Day))
                {
                    agendaView.Days.Add(besteldeActiviteit.Activiteit.Day);
                }
            }

            agendaView.Activities = Bestelling;

            return View(agendaView);
        }

        public ActionResult Reservation()
        {
            return View(new Reservering());
        }

        [HttpPost]
        public ActionResult MakeReservation(Reservering reservation)
        {
            if (ModelState.IsValid)
            {
                List<BesteldeActiviteit> Bestelling = new List<BesteldeActiviteit>();

                if ((List<BesteldeActiviteit>)Session["current_order"] != null)
                {
                    Bestelling = (List<BesteldeActiviteit>)Session["current_order"];
                }

                //Totale prijs van de bestelling berekenen
                foreach( BesteldeActiviteit besteldeactiviteit in Bestelling)
                {
                    reservation.TotaalPrijs += besteldeactiviteit.Price;
                }
                //klant toevoegen aan DB
                homerepository.AddKlant(reservation.Klant);
                //Bestelling toevoegen aan DB
                reservation = homerepository.AddReservation(reservation);
                //Bestelde activiteiten toevoegen aan DB
                foreach (BesteldeActiviteit besteldeactiviteit in Bestelling)
                {
                    besteldeactiviteit.Reservering = reservation;

                    // Kijken of een bestelde activiteit een Jazz Passe- partout is
                    if (besteldeactiviteit.Activiteit.EventType == 0 && besteldeactiviteit.Activiteit.AlternativePrice == 1 || besteldeactiviteit.Activiteit.AlternativePrice == 2)
                    {
                        // Zoja kijken of het een single of all days is, Single is 1 en All days is 2
                        if (besteldeactiviteit.Activiteit.AlternativePrice == 1)
                        {
                            // Alle jazz events ophalen met het dag ID van de single passe partout
                            List<Activity> allJazzDay = jazzRepository.GetJazzByDay(besteldeactiviteit);

                            // voor elke jazz in de allJazzDay de tickets wijzigen in de DB
                            foreach (Activity activity in allJazzDay)
                            {
                                homerepository.ChangeTicketsJazz(activity, besteldeactiviteit);
                            }
                        }
                        else
                        {
                            // Is een AllDaysPass, van elk jazz event moet er dus een ticket af
                            // Onderstaande lijst bevat alle events van Jazz
                            List<Activity> allJazzDay = jazzRepository.GetJazzAllDays(besteldeactiviteit);

                            // voor elke jazz in de allJazzDay de tickets wijzigen in de DB
                            foreach (Activity activity in allJazzDay)
                            {
                                homerepository.ChangeTicketsJazz(activity, besteldeactiviteit);
                            }
                        }
                    }
                    else
                    {
                        besteldeactiviteit.Activiteit.BoughtTickets += besteldeactiviteit.TotalBoughtTickets;
                        homerepository.ChangeTickets(besteldeactiviteit);
                    }

                    // Kijken of activiteit TalkingHaarlem is en of er een vraag is ingevuld
                    if (besteldeactiviteit.Activiteit.EventType == EventType.TalkingHaarlem && besteldeactiviteit.Opmerking != null)
                    {
                        // Split de opmerking in een array twee substrings. Vooraan staat de receiver. Daarna de vraag.
                        String[] questionSubstrings = besteldeactiviteit.Opmerking.Split(new char[] { ':' }, 2);

                        // Maak vraag aan en vul in
                        InterviewQuestion question = new InterviewQuestion()
                        {
                            Receiver = questionSubstrings[0],
                            Content = questionSubstrings[1]
                        };
                        // Opslaan in db
                        talkingRepository.SaveQuestionToDB(question);
                    }

                    homerepository.AddBesteldeActiviteit(besteldeactiviteit);
                }
                return RedirectToAction("Agenda");
            }

            return View("Reservation", reservation);
        }
    }
}