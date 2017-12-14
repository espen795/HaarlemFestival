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
            activity.EventType = EventType.JazzPatronaat;
            activity.BoughtTickets = 0;

            // Price omzetten naar een float.
            float price, alternativePrice;

            if (float.TryParse(collector["Price"].Replace(".", ","), out price))
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else
                ModelState.AddModelError("InvalidPrice", "Please enter a valid price");

            if (collector["AlternativePrice"].ToString() != "")
            {
                if (float.TryParse(collector["AlternativePrice"].Replace(".", ","), out alternativePrice))
                {
                    activity.AlternativePrice = alternativePrice;
                    ModelState["AlternativePrice"].Errors.Clear();
                }
                else
                    ModelState.AddModelError("InvalidAlternativePrice", "Please enter a valid price");
            }

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

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = DateTime.Parse(collector["Date"] + " " + collector["EndSession"]);

                adminRepository.AddEvent(activity);
            }

            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddDinner(Models.Dinner activity, FormCollection collector)
        {
            activity.EventType = EventType.DinnerInHaarlem;

            // Price omzetten naar een float.
            float price, childPrice;

            if (float.TryParse(collector["Price"].Replace(".", ","), out price))
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else
                ModelState.AddModelError("InvalidPrice", "Please enter a valid price");

            if (collector["AlternativePrice"].ToString() != "")
            {
                if (float.TryParse(collector["AlternativePrice"].Replace(".", ","), out childPrice))
                {
                    activity.AlternativePrice = childPrice;
                    ModelState["AlternativePrice"].Errors.Clear();
                }
                else
                    ModelState.AddModelError("InvalidAlternativePrice", "Please enter a valid price");
            }

            if (ModelState.IsValid)
            {
                activity.RestaurantId = Convert.ToInt32(collector["RestaurantId"]);

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = DateTime.Parse(collector["Date"] + " " + collector["EndSession"]);

                adminRepository.AddEvent(activity);

                return RedirectToAction("ManageEvent", "Admin");
            }

            return View(activity);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddTalking(Models.Talking activity, FormCollection collector)
        {
            activity.EventType = EventType.TalkingHaarlem;

            // Price omzetten naar een float.
            float price;

            if (float.TryParse(collector["Price"].Replace(".", ","), out price))
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else
                ModelState.AddModelError("InvalidPrice", "Please enter a valid price");

            if (ModelState.IsValid)
            {
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    activity.Talk.Person1IMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/talking"), activity.Talk.Person1IMG);
                    // file is uploaded
                    Request.Files[0].SaveAs(path);
                }

                if (Request.Files[1] != null)
                {
                    activity.Talk.Person1AltIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/talking"), activity.Talk.Person1AltIMG);
                    // file is uploaded
                    Request.Files[1].SaveAs(path);
                }

                if (Request.Files[2] != null)
                {
                    activity.Talk.Person2IMG = System.IO.Path.GetFileName(Request.Files[2].FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/talking"), activity.Talk.Person2IMG);
                    // file is uploaded
                    Request.Files[2].SaveAs(path);
                }

                
                if (Request.Files[3] != null)
                {
                    activity.Talk.Person2AltIMG = System.IO.Path.GetFileName(Request.Files[3].FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/talking"), activity.Talk.Person2AltIMG);
                    // file is uploaded
                    Request.Files[3].SaveAs(path);
                }

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = DateTime.Parse(collector["Date"] + " " + collector["EndSession"]);

                return RedirectToAction("ManageEvent", "Admin");
            }

            return View(activity);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddHistoric(Models.Historic activity, FormCollection collector)
        {
            activity.EventType = EventType.HistoricHaarlem;
            activity.BoughtTickets = 0;

            // Price omzetten naar een float.
            float price, groupPrice;

            if (float.TryParse(collector["Price"].Replace(".", ","), out price))
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else
                ModelState.AddModelError("InvalidPrice", "Please enter a valid price");

            if (collector["AlternativePrice"].ToString() != "")
            {
                if (float.TryParse(collector["AlternativePrice"].Replace(".", ","), out groupPrice))
                {
                    activity.AlternativePrice = groupPrice;
                    ModelState["AlternativePrice"].Errors.Clear();
                }
                else
                    ModelState.AddModelError("InvalidAlternativePrice", "Please enter a valid price");
            }

            if (ModelState.IsValid)
            {
                activity.GuideId = Convert.ToInt32(collector["GuideId"]);

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = activity.StartSession.AddHours(2.5);

                adminRepository.AddEvent(activity);
            }

            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteEvent(int id)
        {
            adminRepository.DeleteEvent(id);
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