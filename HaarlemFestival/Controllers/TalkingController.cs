﻿using System;
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

        public ActionResult Reservation(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            List<Talking> allTalks = talkingRepository.GetAllTalks();

            ViewBag.OrderId = id;

            TalkViewModel allTalksAndQuestion = new TalkViewModel();

            allTalksAndQuestion.Talkings = allTalks;

            return View(allTalksAndQuestion);
        }

        [HttpPost]
        public ActionResult Reservation(TalkViewModel viewModel)
        {
            List<BesteldeActiviteit> orderedActivity = new List<BesteldeActiviteit>();

            // Session existing?
            if (Session["current_order"] != null)
            {
                orderedActivity.AddRange((List<BesteldeActiviteit>)Session["current_order"]);
            }

            if (ModelState.IsValid)
            {
                List<InterviewQuestion> questions = new List<InterviewQuestion>();

                // Sla lijst van vragen op in sessie
                foreach(Talking talk in viewModel.Talkings)
                {
                    questions.Add(talk.Question);
                }
                Session["interview_question_list"] = questions;
                // Dit moet pas na de bestelling
                //talkingRepository.SaveQuestionToDB(question);
            }
            // Partial view continue or basket
            return RedirectToAction("Index");
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