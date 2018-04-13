using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using HaarlemFestival.Models;
using HaarlemFestival.Repository.Talking;

namespace HaarlemFestival.Controllers
{
    public class TalkingController : Controller
    {
        private ITalkingRepository talkingRepository = new TalkingRepository();

        public ActionResult Index()
        {
            List<Talking> allTalks = talkingRepository.GetAllTalks();

            return View(allTalks);
        }

        public ActionResult Info(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Talking talk = talkingRepository.GetTalkById((int)id);

            return View(talk);
        }

        public ActionResult Reservation()
        {
            TalkViewModel talkView = new TalkViewModel();
            talkView.Talkings = talkingRepository.GetAllTalks();
            // Geeft een lege lijst mee omdat deze gevuld wordt in de view
            talkView.BesteldeActiviteiten = new List<BesteldeActiviteit>();

            return View(talkView);
        }

        [HttpPost]
        public ActionResult Book(TalkViewModel orderedActivitiesAndTalks)
        {
            List<BesteldeActiviteit> orderedActivities = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivities.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            // activity = besteldeactiviteit
            int i = 0;
            // In the session
            foreach (BesteldeActiviteit orderedActivity in orderedActivitiesAndTalks.BesteldeActiviteiten)
            {
                if (orderedActivity.Aantal != 0 || orderedActivity.AantalAlternatief != 0)
                {
                    orderedActivity.TotalBoughtTickets = orderedActivity.Aantal;
                    // Interviewvraag in opmerking stoppen
                    if (orderedActivitiesAndTalks.Talkings[i].QuestionReceiver != null && orderedActivity.Opmerking != null)
                        orderedActivity.Opmerking = string.Format($"{orderedActivitiesAndTalks.Talkings[i].QuestionReceiver}: {orderedActivity.Opmerking}");
                    else
                        orderedActivity.Opmerking = null;
                    // talk ophalen gebaseerd op het id
                    //orderedActivity.Activiteit = talkingRepository.GetOrderTalkById(orderedActivity.Activiteit.ActivityId);
                    orderedActivity.Activiteit = orderedActivitiesAndTalks.Talkings[i];
                    
                    orderedActivities.Add(orderedActivity);
                }
                i++;
            }

            Session["current_order"] = orderedActivities;

            // Partial view continue or basket
            Session["added_to_basket"] = true;

            // Partial view continue or basket
            return RedirectToAction("Reservation", "Talking");
        }
    }
}