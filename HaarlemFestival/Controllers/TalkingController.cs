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
            talkView.BesteldeActiviteiten = new List<BesteldeActiviteit>();

            return View(talkView);
        }

        [HttpPost]
        public ActionResult Book(TalkViewModel activiteit)
        {
            List<BesteldeActiviteit> orderedActivities = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivities.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            int i = 0;
            // In the session
            foreach (BesteldeActiviteit activity in activiteit.BesteldeActiviteiten)
            {
                if (activity.Aantal != 0 || activity.AantalAlternatief != 0)
                {
                    activity.TotalBoughtTickets = activity.Aantal;
                    // Interviewvraag in opmerking stoppen
                    if (activiteit.Talkings[i].QuestionReceiver != null && activity.Opmerking != null)
                        activity.Opmerking = string.Format($"{activiteit.Talkings[i].QuestionReceiver}: {activity.Opmerking}");
                    else
                        activity.Opmerking = null;
                    // talk ophalen gebaseerd op het id
                    activity.Activiteit = talkingRepository.GetOrderTalkById(activity.Activiteit.ActivityId);
                    
                    orderedActivities.Add(activity);
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