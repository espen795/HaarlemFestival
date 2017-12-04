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
    }
}