using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using System.Web.Security;
using HaarlemFestival.Repository.Admin;
namespace HaarlemFestival.Controllers
{
    public class AdminController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();

        // GET: Admin
        public ActionResult Login()
        {
            // Als de sessie met het aantal verkeerde logins nog niet bestaat.
            if(Session["failedLogins"] == null)
            {
                Session["failedLogins"] = 0;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Username,Password")] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = adminRepository.GetAccount(model.Username, model.Password);

                if (account != null) // Als het account bestaat
                {
                    FormsAuthentication.SetAuthCookie(account.Username, false);

                    Session["loggedin_account"] = account;

                    return RedirectToAction("Overview", "Admin"); // Naar de overview pagina sturen
                }
                else // Als het account niet bestaat
                {
                    ModelState.AddModelError("login-error", "Some of the entered information is invalid.");
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Overview()
        {
            return View();
        }


        // Uitloggen
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["loggedin_account"] = null; // Account uit de sessie verwijderen.
            return RedirectToAction("Login", "Admin"); // Gebruiker naar de inlogpagina sturen.
        }

        [Authorize]
        public ActionResult ManageEvent()
        {
            EventData data = adminRepository.GetEventData();

            string selectedEvent = this.Request.QueryString["selectedEvent"];
            ViewData["Restaurants"] = data.Restaurants;
            ViewData["Guides"] = data.Guides;
            ViewData["Languages"] = data.Languages;
            ViewData["Cuisines"] = data.Cuisines;
            ViewData["Dates"] = data.Dates;

            switch (selectedEvent)
            {
                case "Jazz@Patronaat":
                case "DinnerInHaarlem":
                case "TalkingHaarlem":
                case "HistoricHaarlem":
                    ViewBag.ChosenEvent = selectedEvent;
                    break;

                default:
                    ViewBag.ChosenEvent = "";
                    break;
            }

            // TODO: Verkrijg eventlijst - EventList events = repository.GetEvents()
            return View(data);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddJazz(HaarlemFestival.Models.Jazz activity, FormCollection collector)
        {
            activity.EventType = (int)EventType.JazzPatronaat;
            // Model geeft error maar veld mag ook null zijn, daarom clear ik de errors.
            ModelState["Price"].Errors.Clear();
            ModelState["AlternativePrice"].Errors.Clear();

            // Standaard informatie van activity
            activity.AllDayPassPartout = 80;

            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    activity.artist.ArtistImage = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/jazz"), activity.artist.ArtistImage);
                    // file is uploaded
                    file.SaveAs(path);
                }

                // Price omzetten naar een float.
                float price, alternativePrice;

                if (float.TryParse(collector["Price"].Replace(".", ","), out price))
                    activity.Price = price;
                else
                    ModelState.AddModelError("InvalidPrice", "Please enter a valid price");
                
                if (collector["AlternativePrice"].ToString() != "")
                {
                    if (float.TryParse(collector["AlternativePrice"].Replace(".", ","), out alternativePrice))
                        activity.AlternativePrice = alternativePrice;
                    else
                        ModelState.AddModelError("InvalidAlternativePrice", "Please enter a valid price");
                }

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = DateTime.Parse(collector["Date"] + " " + collector["EndSession"]);

                adminRepository.AddEvent(activity);
            }

            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddDinner(HaarlemFestival.Models.Dinner activity, FormCollection collector, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ManageEvent", "Admin");
            }

            return View(activity);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddTalking(HaarlemFestival.Models.Talking activity, FormCollection collector, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ManageEvent", "Admin");
            }

            return View(activity);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddHistoric(HaarlemFestival.Models.Historic activity, FormCollection collector, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ManageEvent", "Admin");
            }

            return View(activity);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteEvent(int id)
        {
            adminRepository.DeleteEvent(id);
            ViewBag.Status = "Deleted";
            return RedirectToAction("ManageEvent","Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateEvent(Activity activity)
        {
            adminRepository.UpdateEvent(activity);
            ViewBag.Status = "Updated";
            return RedirectToAction("ManageEvent","Admin");
        }

        [Authorize]
        public ActionResult Download()
        {
            return View();
        }


        [Authorize]
        public ActionResult _UpdateEvent(int id)
        {
            Activity activity = adminRepository.GetActivity(id);
            return PartialView(activity);
        }

        public ActionResult _JazzPartial(Jazz model)
        {
            return PartialView(model);
        }

        public ActionResult _DinnerPartial(Dinner model)
        {
            return PartialView(model);
        }

        public ActionResult _TalkingPartial(Talking model)
        {
            return PartialView(model);
        }

        public ActionResult _HistoricPartial(Historic model)
        {
            return PartialView(model);
        }
    }
}