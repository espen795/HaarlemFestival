using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            List<Talk> allTalks = talkingRepository.GetAllTalks();

            return View(allTalks);
            //return View();
        }

        public ActionResult Info(int? id)
        {
            Talk talk = talkingRepository.GetTalkById((int)id);

            return View(talk);
        }

        public ActionResult Reservation()
        {
            return View();
        }
    }
}