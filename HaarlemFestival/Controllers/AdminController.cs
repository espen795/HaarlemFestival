using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using System.Web.Security;

namespace HaarlemFestival.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Login()
        {
            // Als de sessie met het aantal verkeerde logins nog niet bestaat.
            if(Session["failedLogins"] == null)
            {
                Session["failedLogins"] = 0;
            }

            DateTime now = DateTime.Now;

            // Als er 3x verkeerd is ingelogd.
            if ((int)Session["failedLogins"] == 3)
                ModelState.AddModelError("login-block", "Some of the entered information is invalid. Please try again in 5 minutes (" + now.AddMinutes(5).ToString() + ")");

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
            // TODO: Verkrijg eventlijst - EventList events = repository.GetEvents()
            return View();
        }

        [HttpPost]
        public ActionResult DeleteEvent()
        {
            // TODO: Repository aanmaken en DB Item verwijderen.

            return RedirectToAction("ManageEvent","Admin");
        }

        [HttpPost]
        public ActionResult UpdateEvent()
        {
            // TODO: Repository aanmaken en DB Item updaten.

            return RedirectToAction("ManageEvent","Admin");
        }



        // PartialViews - W.I.P
        public ActionResult _updateEvent()
        {
            return PartialView();
        }

        public ActionResult _deleteConfirm()
        {
            return PartialView();
        }
    }
}