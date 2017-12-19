using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using HaarlemFestival.Repository.Jazz;

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

        public ActionResult DayOverview()
        {
            List<Jazz> allJazz = jazzRepository.GetAllJazzs();

            return View(allJazz);
        }
    }
}