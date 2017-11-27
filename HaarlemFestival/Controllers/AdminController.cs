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

            // Als er 3x verkeerd is ingelogd.
            if ((int)Session["failedLogins"] == 3)
                ModelState.AddModelError("login-block", "Some of the entered information is invalid. Please try again in 300 seconds (5 minutes)");

            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Username,Password")] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account();

                if (account != null) // Als het account bestaat
                {
                    FormsAuthentication.SetAuthCookie(account.Username, false);

                    Session["loggedin_account"] = account;

                    return RedirectToAction("Overview", "Admin"); // De gebruiker naar zijn of haar contactenlijst sturen
                }
                else // Als het account niet bestaat
                {
                    ModelState.AddModelError("login-error", "Some of the entered information is invalid.");
                }
            }

            return View(model);
        }

        public ActionResult Overview()
        {
            return View();
        }

        // Uitloggen
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["loggedin_account"] = null; // Account uit de sessie verwijderen.
            return RedirectToAction("Login", "Admin"); // Gebruiker naar de inlogpagina sturen.
        }

        public ActionResult ManageEvent()
        {
            string selectedEvent = this.Request.QueryString["selectedEvent"];

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
            return View();
        }

        [HttpPost]
        public ActionResult AddEvent(Activity activity)
        {
            if (ModelState.IsValid)
            {
                adminRepository.AddEvent(activity);
                ViewBag.Status = "Deleted";
                return RedirectToAction("ManageEvent", "Admin");
            }

            return View(activity);
        }

        [HttpPost]
        public ActionResult DeleteEvent(Activity activity)
        {
            adminRepository.DeleteEvent(activity);
            ViewBag.Status = "Deleted";
            return RedirectToAction("ManageEvent","Admin");
        }

        [HttpPost]
        public ActionResult UpdateEvent(Activity activity)
        {
            adminRepository.UpdateEvent(activity);
            ViewBag.Status = "Updated";
            return RedirectToAction("ManageEvent","Admin");
        }

        public ActionResult Download()
        {
            return View();
        }
    }
}