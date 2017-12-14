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
            List<Artist> allArtist = jazzRepository.GetAllArtists();
            List<Jazz> allJazz = jazzRepository.GetAllJazzs();

            JazzViewModels viewModel = new JazzViewModels();
            viewModel.Artists = allArtist;
            viewModel.Jazz = allJazz;

            return View();
        }

        public ActionResult DayOverview()
        {
            return View();
        }
    }
}