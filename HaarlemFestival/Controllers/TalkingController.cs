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
            //return View();
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
            TalkViewModel allTalksAndQuestion = new TalkViewModel();
            allTalksAndQuestion.Talkings = talkingRepository.GetAllTalks();
            allTalksAndQuestion.BesteldeActiviteiten = new List<BesteldeActiviteit>();

            return View(allTalksAndQuestion);
        }

        [HttpPost]
        public ActionResult Book(TalkViewModel activiteit)
        {
            List<BesteldeActiviteit> orderedActivity = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            int i = 0;
            // In the session
            foreach (BesteldeActiviteit activity in activiteit.BesteldeActiviteiten)
            {
                if (activity.Aantal != 0 || activity.AantalAlternatief != 0)
                {
                    activity.TotalBoughtTickets = activity.Aantal;
                    // Interviewvraag in opmerking stoppen
                    activity.Opmerking = string.Format($"{activiteit.Talkings[i].QuestionReceiver}: {activity.Opmerking}");
                    // talk ophalen gebaseerd op het id
                    activity.Activiteit = talkingRepository.GetOrderTalkById(activity.Activiteit.ActivityId);
                    
                    orderedActivity.Add(activity);
                }
                i++;
            }

            Session["current_order"] = orderedActivity;

            // Partial view continue or basket
            Session["added_to_basket"] = true;

            // Partial view continue or basket
            return RedirectToAction("Reservation", "Talking");
        }

        private bool CheckInterviewQuestion(InterviewQuestion question)
        {
            if (question.Content == null)
                return false;
            else if (question.Receiver == null)
                return false;
            return true;
        }
    }
}