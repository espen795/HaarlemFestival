﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using System.Web.Security;
using HaarlemFestival.Repository.Admin;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Security.Principal;

namespace HaarlemFestival.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        IAdminRepository adminRepository = new AdminRepository();

        #region Views
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Admin");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult ManageEvent()
        {
            // Evenementdata ophalen uit de database
            EventData data = adminRepository.GetEventData();
            data.Filters.dateFilter = GetDateModel(data.Filters.days); // DateFilter aanmaken aan de hand van de lijst met days

            string selectedEvent = this.Request.QueryString["selectedEvent"]; // Geselecteerde evenement uit de URL halen

            switch (selectedEvent)
            {
                //  Als er een evenement uit de URL gehaald kan worden
                case "Jazz@Patronaat":
                case "DinnerInHaarlem":
                case "TalkingHaarlem":
                case "HistoricHaarlem":
                    ViewBag.ChosenEvent = selectedEvent;
                    break;

                default: // Wanneer er geen evenement uit de URL gehaald kan worden
                    ViewBag.ChosenEvent = "";
                    break;
            }

            return View(data);
        }

        public ActionResult TicketSalesInformation()
        {
            Models.Filters filters = adminRepository.GetFilters();
            filters.dateFilter = GetDateModel(filters.days);

            TicketSalesViewModel viewModel = new TicketSalesViewModel()
            {
                Activities = adminRepository.GetActivities(),
                BesteldeActiviteiten = adminRepository.GetBesteldeActivities(),
                Filters = filters
            };

            List<Activity> activities = adminRepository.GetActivities();
            return View(viewModel);
        }

        public ActionResult AnswerContactmessage()
        {
            AnswerContactViewModel viewModel = new AnswerContactViewModel() // ViewModel aanmaken
            {
                Messages = adminRepository.GetContactMessages() // Ingestuurde Contact Berichten ophalen.
            };

            return View(viewModel);
        }

        public ActionResult CheckInterviewQuestions()
        {
            List<InterviewQuestion> interviewQuestions = adminRepository.GetInterviewQuestions(); // Interview Berichten ophalen.

            return View(interviewQuestions);
        }

        public ActionResult ManageAccounts()
        {
            string[] allowedRoles = new string[] { "Owner" }; // Array met de rollen die deze pagina mogen bezoeken
            if(!UserAuthorized(allowedRoles)) // Kijken of de gebruiker naar de pagina mag navigeren
                return RedirectToAction("Overview", "Admin"); // Als de gebruiker niet naar de pagina mag navigeren wordt hij teruggestuurd naar de overview pagina

            ManageAccountViewModel viewModel = new ManageAccountViewModel() // ViewModel aanmaken
            {
                AccountList = adminRepository.GetAccounts(),
                Roles = adminRepository.GetRoles()
            };

            return View(viewModel);
        }
        #endregion

        #region Login/Logout
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([Bind(Include = "Username,Password")] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = adminRepository.GetAccount(model.Username, model.Password);

                if (account != null) // Als het account bestaat.
                {
                    FormsAuthentication.SetAuthCookie(account.Username, false);
                    string[] role = new string[1] { account.Role.RoleName };

                    HttpContext.User = new GenericPrincipal(new GenericIdentity(account.Username, "Forms"), role);

                    Session["loggedin_account"] = account; // Account toevoegen aan de session.

                    return RedirectToAction("Overview", "Admin"); // Naar de overview pagina sturen.
                }
                else // Als het account niet bestaat.
                {
                    ModelState.AddModelError("login-error", "Some of the entered information is invalid.");
                }
            }

            return View(model);
        }

        // Uitloggen
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["loggedin_account"] = null; // Account uit de sessie verwijderen.
            return RedirectToAction("Login", "Admin"); // Gebruiker naar de inlogpagina sturen.
        }
        #endregion

        #region AddRegion
        [HttpPost]
        public JsonResult AddJazz(HaarlemFestival.Models.Jazz activity, FormCollection collector)
        {
            // Standaard informatie van activity
            activity.EventType = EventType.JazzPatronaat;
            activity.BoughtTickets = 0;

            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);


            if (ModelState.IsValid)
            {
                // Image ophalen en uploaden
                try
                {
                    activity.artist.ArtistImage = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    UploadImage(Request.Files[0], "Jazz");
                }
                catch (Exception) { }

                // Datum en tijd ophalen
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.Day.Date.Add(TimeSpan.Parse(collector["EndSession"]));

                // Evenement toevoegen
                adminRepository.AddEvent(activity);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddRestaurant(Models.Restaurant restaurant, FormCollection collector)
        {
            // Proberen om de rating op te halen (Kan leeggemaakt worden in formulier)
            try
            {
                if (restaurant.Rating.Length != 0)
                    restaurant.Rating = restaurant.Rating + "/5";
                else
                    ModelState.AddModelError("NoRating", "Please enter a valid rating.");
            }
            catch (NullReferenceException)
            {
                ModelState.AddModelError("NoRating", "Please enter a rating.");
            }

            UpdateDisplayPrice(restaurant, collector);
            GetCuisines(restaurant, collector);

            if (ModelState.IsValid)
            {
                // Images ophalen en uploaden
                try
                {
                    restaurant.FoodIMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    UploadImage(Request.Files[0], "Restaurant");
                }
                catch (Exception) { }

                try
                {
                    restaurant.LocationIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    UploadImage(Request.Files[1], "Restaurant");
                }
                catch (Exception) { }

                // Restaurant toevoegen
                adminRepository.AddRestaurant(restaurant);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddDinner(Models.Dinner activity, FormCollection collector)
        {
            // Standaard informatie van activity
            activity.EventType = EventType.DinnerInHaarlem;

            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

            if (ModelState.IsValid)
            {
                // RestaurantId ophalen
                activity.RestaurantId = Convert.ToInt32(collector["RestaurantId"]);

                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.Day.Date.Add(TimeSpan.Parse(collector["EndSession"]));

                // Dinner evenement toevoegen
                adminRepository.AddEvent(activity);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTalking(Models.Talking activity, FormCollection collector)
        {
            // Standaard informatie van activity
            activity.EventType = EventType.TalkingHaarlem;
            activity.AlternativePrice = null;

            // Prijs ophalen.
            UpdatePrice(activity, collector);

            if (ModelState.IsValid)
            {
                // Images ophalen en uploaden
                try
                {
                    activity.Talk.Person1IMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    UploadImage(Request.Files[0], "Talking");
                }
                catch (Exception) { }

                try
                {
                    activity.Talk.Person1AltIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    UploadImage(Request.Files[1], "Talking");
                }
                catch (Exception) { }

                try
                {
                    activity.Talk.Person2IMG = System.IO.Path.GetFileName(Request.Files[2].FileName);
                    UploadImage(Request.Files[2], "Talking");
                }
                catch (Exception) { }

                try
                {
                    activity.Talk.Person2AltIMG = System.IO.Path.GetFileName(Request.Files[3].FileName);
                    UploadImage(Request.Files[3], "Talking");
                }
                catch (Exception) { }

                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.Day.Date.Add(TimeSpan.Parse(collector["EndSession"]));

                //Talking evenement toevoegen.
                adminRepository.AddEvent(activity);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddGuide(Models.Guide guide, FormCollection collector)
        {
            if (ModelState.IsValid)
            {
                // Guide toevoegen.
                adminRepository.AddGuide(guide);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddHistoric(Models.Historic activity, FormCollection collector)
        {
            // Standaard informatie van activity
            activity.EventType = EventType.HistoricHaarlem;
            activity.BoughtTickets = 0;

            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

            if (ModelState.IsValid)
            {
                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.StartSession.AddHours(2.5);

                // Historic evenement toevoegen.
                adminRepository.AddEvent(activity);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddAccount(Models.ManageAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                adminRepository.AddAccount(viewModel.Account);
                Session["Data_Added"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region UpdateRegion

        [HttpPost]
        public ActionResult UpdateEvent(Activity activity)
        {
            // Evenement updaten.
            adminRepository.UpdateEvent(activity);
            return RedirectToAction("ManageEvent", "Admin");
        }

        public ActionResult _UpdateData(int id)
        {
            string type = this.Request["type"].ToLower(); // Type ophalen.

            // Aan de hand van het opgehaalde type de desbetreffende PartialView terugsturen.
            switch (type)
            {
                case "jazz":
                    return PartialView("_UpdateJazzPartial", id);

                case "restaurant":
                    return PartialView("_UpdateRestaurantPartial", id);

                case "dinner":
                    return PartialView("_UpdateDinnerPartial", id);

                case "talking":
                    return PartialView("_UpdateTalkingPartial", id);

                case "guide":
                    return PartialView("_UpdateGuidePartial", id);

                case "historic":
                    return PartialView("_UpdateHistoricPartial", id);

                case "account":
                    return PartialView("_UpdateAccountPartial", id);

                default:
                    return RedirectToAction("ManageEvent", "Admin");
            }
        }

        [HttpPost]
        public JsonResult UpdateJazz(Models.Jazz activity, FormCollection collector)
        {
            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);
            activity.artist.ArtistId = (int)activity.ArtistId;
            if (ModelState.IsValid)
            {
                // Image ophalen en uploaden.
                try
                {
                    if (Request.Files[0].ContentLength > 0 && Request.Files[0] != null)
                    {
                        UploadImage(Request.Files[0], "Jazz");
                        activity.artist.ArtistImage = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    }
                }
                catch (Exception) { }

                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.Day.Date.Add(TimeSpan.Parse(collector["EndSession"]));

                // Jazz Evenement Updaten.
                adminRepository.UpdateEvent(activity);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateRestaurant(Models.Restaurant restaurant, FormCollection collector)
        {
            //Proberen om de rating op te halen.
            if (restaurant.Rating != null)
                restaurant.Rating = restaurant.Rating + "/5";
            else
                ModelState.AddModelError("NoRating", "Please enter a valid rating");

            UpdateDisplayPrice(restaurant, collector);
            GetCuisines(restaurant, collector);

            if (ModelState.IsValid)
            {
                // Images ophalen en uploaden.
                try
                {
                    if (Request.Files[0].ContentLength > 0 && Request.Files[0] != null)
                    {
                        UploadImage(Request.Files[0], "Restaurant");
                        restaurant.FoodIMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    }
                }
                catch (Exception) { }

                try
                {
                    if (Request.Files[1].ContentLength > 0 && Request.Files[1] != null)
                    {
                        UploadImage(Request.Files[1], "Restaurant");
                        restaurant.LocationIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    }
                }
                catch (Exception) { }

                // Restaurant data updaten.
                adminRepository.UpdateRestaurant(restaurant);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDinner(Models.Dinner activity, FormCollection collector)
        {
            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

            if (ModelState.IsValid)
            {
                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.Day.Date.Add(TimeSpan.Parse(collector["EndSession"]));

                // Dinner data ophalen.
                adminRepository.UpdateEvent(activity);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTalking(Models.Talking activity, FormCollection collector)
        {
            // Prijs ophalen.
            UpdatePrice(activity, collector);

            if (ModelState.IsValid)
            {
                // Images ophalen en uploaden.
                try
                {
                    if (Request.Files[0].ContentLength > 0 && Request.Files[0] != null)
                    {
                        UploadImage(Request.Files[0], "Talking");
                        activity.Talk.Person1IMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    }
                }
                catch (Exception) { }

                try
                {
                    if (Request.Files[1].ContentLength > 0 && Request.Files[1] != null)
                    {
                        UploadImage(Request.Files[1], "Talking");
                        activity.Talk.Person1AltIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    }
                }
                catch (Exception) { }

                try
                {
                    if (Request.Files[2].ContentLength > 0 && Request.Files[2] != null)
                    {
                        UploadImage(Request.Files[2], "Talking");
                        activity.Talk.Person2IMG = System.IO.Path.GetFileName(Request.Files[2].FileName);
                    }
                }
                catch (Exception) { }

                try
                {
                    if (Request.Files[3].ContentLength > 0 && Request.Files[3] != null)
                    {
                        UploadImage(Request.Files[3], "Talking");
                        activity.Talk.Person2AltIMG = System.IO.Path.GetFileName(Request.Files[3].FileName);
                    }
                }
                catch (Exception) { }

                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.Day.Date.Add(TimeSpan.Parse(collector["EndSession"]));

                // Talking data updaten.
                adminRepository.UpdateEvent(activity);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateGuide(Models.Guide guide, FormCollection collector)
        {
            if (ModelState.IsValid)
            {
                // Guide updaten.
                adminRepository.UpdateGuide(guide);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateHistoric(Models.Historic activity, FormCollection collector)
        {
            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

            // Guide ophalen uit de database
            activity.Guide = adminRepository.GetGuide(activity.GuideId);

            // Als de guide niet leeg is
            if (activity.Guide != null)
            {
                // ModelState errors weghalen.
                ModelState["GuideId"].Errors.Clear();
                ModelState["Guide.GuideId"].Errors.Clear();
                ModelState["Guide.GuideName"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                // Datum en tijd ophalen.
                activity.Day = adminRepository.GetDay(activity.Day.DayId);
                activity.StartSession = activity.Day.Date.Add(TimeSpan.Parse(collector["StartSession"]));
                activity.EndSession = activity.StartSession.AddHours(2.5);

                // Historic data updaten.
                adminRepository.UpdateEvent(activity);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateAccount(Models.ManageAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                adminRepository.UpdateAccount(viewModel.Account);
                Session["Data_Updated"] = true;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        // Methoden voor meergebruikte functies.
        private void UploadImage(HttpPostedFileBase file, string type)
        {
            string fileName = System.IO.Path.GetFileName(file.FileName); // Bestandsnaam ophalen.
            string path = "";

            // Pad voor bestand ophalen.
            switch (type)
            {
                case "Jazz":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/jazz/artiesten"), fileName);
                    break;
                case "Restaurant":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/dinner/restaurants"), fileName);
                    break;
                case "Dinner":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/dinner/restaurants"), fileName);
                    break;
                case "Talking":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/talking"), fileName);
                    break;
                case "Guide":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/historic"), fileName);
                    break;
                case "Historic":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/historic"), fileName);
                    break;
            }

            // Bestand uploaden.
            file.SaveAs(path);
        }

        private void UpdateDisplayPrice(Models.Restaurant restaurant, FormCollection collector)
        {
            float price;
            if (float.TryParse(collector["DisplayPrice"].Replace(".", ","), out price))
            {
                ModelState["DisplayPrice"].Errors.Clear();
                restaurant.DisplayPrice = price;
            }
            else if (collector["DisplayPrice"].ToString().Length > 0)
                ModelState.AddModelError("InvalidDisplayPrice", "Please enter a valid Displayprice.");
            else if (collector["DisplayPrice"].ToString().Length == 0)
                ModelState.AddModelError("NoDisplayPrice", "Please enter a Standard Price");
        }

        private void UpdatePrice(Activity activity, FormCollection collector)
        {
            float price;
            if (float.TryParse(collector["Price"].Replace(".", ","), out price)) // Als de prijs omgezet kan worden naar een float.
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else if (collector["Price"].ToString().Length > 0) // Als er een prijs is ingevoerd maar niet omgezet kan worden.
                ModelState.AddModelError("InvalidPrice", "Please enter a valid price.");
        }

        private void UpdateAlternativePrice(Activity activity, FormCollection collector)
        {
            float alternativePrice;
            if (collector["AlternativePrice"].ToString() != "") // Als de alternatieve prijs niet leeg is.
            {
                if (float.TryParse(collector["AlternativePrice"].Replace(".", ","), out alternativePrice)) // Als de alternatieve prijs omgezet kan worden naar een float.
                {
                    activity.AlternativePrice = alternativePrice;
                    ModelState["AlternativePrice"].Errors.Clear();
                }
                else // Als de alternatieve prijs niet omgezet kan worden naar een float.
                    ModelState.AddModelError("InvalidAlternativePrice", "Please enter a valid price");
            }
        }
        #endregion

        #region DeleteRegion
        [HttpPost]
        public ActionResult DeleteEvent(int id)
        {
            // Evenement verwijderen.
            adminRepository.DeleteEvent(id);
            Session["Data_Deleted"] = true;
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        public ActionResult DeleteRestaurant(int id)
        {
            // Restaurant verwijderen.
            adminRepository.DeleteRestaurant(id);
            Session["Data_Deleted"] = true;
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        public ActionResult DeleteGuide(int id)
        {
            // Guide verwijderen.
            adminRepository.DeleteGuide(id);
            Session["Data_Deleted"] = true;
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        public ActionResult DeleteAccount(int id)
        {
            adminRepository.DeleteAccount(id);
            Session["Data_Deleted"] = true;
            return RedirectToAction("ManageAccounts", "Admin");
        }
        #endregion

        #region PartialViews
        public ActionResult _JazzPartial(int? id)
        {
            ViewData["Dates"] = GetDateModel(adminRepository.GetDates()); // Datums ophalen.

            Jazz jazz;
            if (id != null) // Als er een id is meegegeven.
            {
                // Jazz Evenement ophalen.
                jazz = adminRepository.GetActivity((int)id) as Jazz;
                jazz.artist.ArtistInformation = Regex.Replace(jazz.artist.ArtistInformation, @"<(.|\n)*?>", string.Empty); // HTML Tags uit de description halen.
            }
            else // Als er geen id is meegegeven.
            {
                jazz = new Jazz(); // Nieuwe jazz data aanmaken.
            }

            return PartialView(jazz); // Jazz data terugsturen.
        }

        public ActionResult _RestaurantPartial(int? id)
        {
            // Cuisines ophalen.
            ViewData["Cuisines"] = adminRepository.GetCuisines();

            Restaurant restaurant;
            if (id != null) // Als er een id is meegegeven.
            {
                // Restaurant data ophalen.
                restaurant = adminRepository.GetRestaurant((int)id);
                restaurant.Rating = restaurant.Rating.Substring(0, 1);
            }
            else // Als er geen id is meegegeven.
            {
                restaurant = new Restaurant(); // Nieuwe Restaurant data aanmaken
            }

            return PartialView(restaurant); // Restaurant data terugsturen.
        }

        public ActionResult _DinnerPartial(int? id)
        {
            // Restaurants en Datums ophalen.
            ViewData["Restaurants"] = adminRepository.GetRestaurants();
            ViewData["Dates"] = GetDateModel(adminRepository.GetDates());

            Dinner dinner;
            if (id != null) // Als er een id is meegegeven.
            {
                // Dinner evenement ophalen.
                dinner = adminRepository.GetActivity((int)id) as Dinner;
            }
            else // Als er geen id een meegegeven.
            {
                dinner = new Dinner(); // Nieuwe Dinner data aanmaken
            }

            return PartialView(dinner); // Dinner Data terugsturen
        }

        public ActionResult _TalkingPartial(int? id)
        {
            // Datums ophalen.
            ViewData["Dates"] = GetDateModel(adminRepository.GetDates());

            Talking talking;
            if (id != null) // Als er een id is meegegeven.
            {
                // Talking data ophalen.
                talking = adminRepository.GetActivity((int)id) as Talking;
            }
            else // Als er geen id is meegegeven.
            {
                talking = new Talking(); // Nieuwe Talking data aanmaken.
            }

            return PartialView(talking); // Talking data terugsturen.
        }

        public ActionResult _GuidePartial(int? id)
        {
            Guide guide;
            if (id != null) // Als er een id is meegegeven.
            {
                // Guide data ophalen.
                guide = adminRepository.GetGuide((int)id);
            }
            else // Als er geen id is meegegeven
            {
                guide = new Guide(); // Nieuwe Guide data aanmaken.
            }

            return PartialView(guide); // Guide data terugsturen.
        }

        public ActionResult _HistoricPartial(int? id)
        {
            // Guides en Datums ophalen.
            ViewData["Guides"] = adminRepository.GetGuides();
            ViewData["Dates"] = GetDateModel(adminRepository.GetDates());

            Historic historic;
            if (id != null) // Als er een id is meegegeven.
            {
                // Historic data ophalen.
                historic = adminRepository.GetActivity((int)id) as Historic;
            }
            else // ALs er geen id is meegegeven.
            {
                historic = new Historic(); // Nieuwe Historic data aanmaken.
            }

            return PartialView(historic); // Historic data terugsturen.
        }

        public ActionResult _AnswerQuestionPartial(int id)
        {
            ContactMessage message = adminRepository.GetContactMessage(id);
            return PartialView(message);
        }

        public ActionResult _AccountPartial(int? id)
        {
            Account account;
            if (id != null)
                account = adminRepository.GetAccountById((int)id);
            else
                account = new Account();

            ManageAccountViewModel viewModel = new ManageAccountViewModel
            {
                Account = account,
                Roles = adminRepository.GetRoles()
            };


            return PartialView(viewModel);
        }

        public ActionResult _AddAccountPartial()
        {
            return PartialView();
        }

        public ActionResult _UpdateAccountPartial(int id)
        {
            return PartialView(id);
        }
        #endregion

        #region ExcelBestand

        public JsonResult DownloadTicketSalesInformation()
        {
            string saveLocation = CreateExcelFile(); // Actie om een Excel Bestand aan te maken en de locatie van het bestand terug te sturen

            return Json(new { location = saveLocation }); // De locatie van het aangemaakte bestand terugsturen
        }

        private string CreateExcelFile()
        {
            // Lijst met activities ophalen.
            List<Activity> activities = adminRepository.GetActivities();

            Application excel = new Application
            {
                // Excel niet zichtbaar maken
                Visible = false,
                DisplayAlerts = false
            };

            // Nieuwe Workbook aan het excel bestand toevoegen.
            Workbook data = excel.Workbooks.Add(Type.Missing);

            // Nieuwe Sheets ophalen en toevoegen
            data = GetGeneralWorksheet(data, activities);
            data = GetJazzWorksheet(data, activities);
            data = GetDinnerWorksheet(data, activities);
            data = GetTalkingWorksheet(data, activities);
            data = GetHistoricWorksheet(data, activities);

            // Bestand Opslaan.

            data.SaveAs("Ticket_Sales_Information.xlsx");

            string saveLocation = data.FullNameURLEncoded;
            // Bestand sluiten.
            data.Close();

            return saveLocation; // De bestandslocatie terugsturen
        }

        private Workbook GetGeneralWorksheet(Workbook data, List<Activity> activities)
        {
            Worksheet generalSheet = (Worksheet)data.Worksheets.Add(); // Nieuwe Sheet aan het bestand toevoegen
            generalSheet.Name = "General Information"; // Naam aan de sheet geven

            var heading = generalSheet.Range[generalSheet.Cells[1, 1], generalSheet.Cells[1, 5]]; // De headers van het document selecteren
            heading.Interior.Color = Color.Red; // Rode achtergrondkleur aan de header geven
            heading.Font.Color = Color.White; // Witte tekstkleur aan de header geven

            // Heading text neerzetten
            generalSheet.Cells[1, 1] = "Event";
            generalSheet.Cells[1, 2] = "Total Tickets/Seats";
            generalSheet.Cells[1, 3] = "Available Tickets/Seats";
            generalSheet.Cells[1, 4] = "Percentage of Tickets Sold";
            generalSheet.Cells[1, 5] = "Income";

            var enums = Enum.GetValues(typeof(EventType)).Cast<EventType>();

            int rowCount = 2; // int om aan te geven vanaf welke rij de data moet beginnen

            // Data per evenement neerzetten
            foreach (EventType type in enums)
            {
                string eventType = "";
                switch (type)
                {
                    case EventType.JazzPatronaat:
                        eventType = "Jazz@Patronaat";
                        break;

                    case EventType.DinnerInHaarlem:
                        eventType = "Dinner In Haarlem";
                        break;

                    case EventType.TalkingHaarlem:
                        eventType = "Talking Haarlem";
                        break;

                    case EventType.HistoricHaarlem:
                        eventType = "Historic Haarlem";
                        break;
                }

                // EvenementType weergeven.
                generalSheet.Cells[rowCount, 1] = eventType;

                // Totale aantal tickets ophalen en weergeven.
                int totalTickets = activities.Where(a => a.EventType == type).Sum(a => a.TotalTickets);
                generalSheet.Cells[rowCount, 2] = totalTickets;

                // Gekochte aantal tickets ophalen en weergeven.
                int soldTickets = activities.Where(a => a.EventType == type).Sum(a => a.TotalTickets - a.BoughtTickets);
                generalSheet.Cells[rowCount, 3] = soldTickets;

                // Percentage van gekochte tickets uitrekenen en weergeven.
                decimal availablePercentage = Math.Round((decimal)soldTickets, 2) / Math.Round((decimal)totalTickets, 2) * 100;
                decimal boughtPercentage = 100 - availablePercentage;
                generalSheet.Cells[rowCount, 4] = boughtPercentage;

                // Achtergrondkleur van Percentage gekochte tickets neerzetten.
                if (boughtPercentage > 80)
                    generalSheet.Cells[rowCount, 4].Interior.Color = Color.Green;
                else if (boughtPercentage > 60)
                    generalSheet.Cells[rowCount, 4].Interior.Color = Color.Yellow;
                else
                    generalSheet.Cells[rowCount, 4].Interior.Color = Color.OrangeRed;

                // Totale Inkomen weergeven.
                generalSheet.Cells[rowCount, 5] = adminRepository.GetIncomeByType(type).ToString("0.00");

                rowCount++;
            }

            generalSheet.Columns.AutoFit(); // Ervoor zorgen dat de informatie in de sheet goed past in de vakjes

            return data; // Excelbestand terugsturen
        }

        private Workbook GetJazzWorksheet(Workbook data, List<Activity> activities)
        {
            Worksheet jazzSheet = (Worksheet)data.Worksheets.Add(); // Nieuwe Sheet aan het bestand toevoegen
            jazzSheet.Name = "Jazz@Patronaat"; // Naam aan de sheet geven

            var heading = jazzSheet.Range[jazzSheet.Cells[1, 1], jazzSheet.Cells[1, 8]]; // De headers van het document selecteren
            heading.Interior.Color = Color.Red; // Rode achtergrondkleur aan de header geven
            heading.Font.Color = Color.White; // Witte tekstkleur aan de header geven

            // Heading text neerzetten
            jazzSheet.Cells[1, 1] = "Band Name";
            jazzSheet.Cells[1, 2] = "Date";
            jazzSheet.Cells[1, 3] = "Time";
            jazzSheet.Cells[1, 4] = "Total Seats";
            jazzSheet.Cells[1, 5] = "Tickets Sold";
            jazzSheet.Cells[1, 6] = "Available Tickets";
            jazzSheet.Cells[1, 7] = "Price";
            jazzSheet.Cells[1, 8] = "Income";

            int rowCount = 2; // int om aan te geven vanaf welke rij de data moet beginnen

            // Data toevoegen aan de sheet
            foreach (Jazz activity in activities.OfType<Jazz>())
            {
                jazzSheet.Cells[rowCount, 1] = activity.artist.ArtistName;
                jazzSheet.Cells[rowCount, 2] = activity.StartSession.ToString("dddd dd-MM-yyyy");
                jazzSheet.Cells[rowCount, 3] = activity.StartSession.ToString("H:mm") + " - " + activity.EndSession.ToString("H:mm");
                jazzSheet.Cells[rowCount, 4] = activity.TotalTickets;
                jazzSheet.Cells[rowCount, 5] = activity.BoughtTickets;
                jazzSheet.Cells[rowCount, 6] = activity.TotalTickets - activity.BoughtTickets;
                jazzSheet.Cells[rowCount, 7] = activity.Price;
                jazzSheet.Cells[rowCount, 8] = adminRepository.GetIncomeById(activity.ActivityId).ToString("0.00");
                rowCount++;
            }

            jazzSheet.Columns.AutoFit(); // Ervoor zorgen dat de informatie in de sheet goed past in de vakjes

            return data; // Excelbestand terugsturen
        }

        private Workbook GetDinnerWorksheet(Workbook data, List<Activity> activities)
        {
            Worksheet dinnerSheet = (Worksheet)data.Worksheets.Add(); // Nieuwe Sheet aan het bestand toevoegen
            dinnerSheet.Name = "Dinner in Haarlem"; // Naam aan de sheet geven

            var heading = dinnerSheet.Range[dinnerSheet.Cells[1, 1], dinnerSheet.Cells[1, 8]]; // De headers van het document selecteren
            heading.Interior.Color = Color.Red; // Rode achtergrondkleur aan de header geven
            heading.Font.Color = Color.White; // Witte tekstkleur aan de header geven

            // Heading text neerzetten
            dinnerSheet.Cells[1, 1] = "Restaurant";
            dinnerSheet.Cells[1, 2] = "Date";
            dinnerSheet.Cells[1, 3] = "Time";
            dinnerSheet.Cells[1, 4] = "Total Seats";
            dinnerSheet.Cells[1, 5] = "Seats Sold";
            dinnerSheet.Cells[1, 6] = "Available Seats";
            dinnerSheet.Cells[1, 7] = "Price";
            dinnerSheet.Cells[1, 8] = "Income";

            int rowCount = 2; // int om aan te geven vanaf welke rij de data moet beginnen

            // Data toevoegen aan de sheet
            foreach (Dinner activity in activities.OfType<Dinner>())
            {
                dinnerSheet.Cells[rowCount, 1] = activity.Restaurant.Naam;
                dinnerSheet.Cells[rowCount, 2] = activity.StartSession.ToString("dddd dd-MM-yyyy");
                dinnerSheet.Cells[rowCount, 3] = activity.StartSession.ToString("H:mm") + " - " + activity.EndSession.ToString("H:mm");
                dinnerSheet.Cells[rowCount, 4] = activity.TotalTickets;
                dinnerSheet.Cells[rowCount, 5] = activity.BoughtTickets;
                dinnerSheet.Cells[rowCount, 6] = activity.TotalTickets - activity.BoughtTickets;
                dinnerSheet.Cells[rowCount, 7] = activity.Price;
                dinnerSheet.Cells[rowCount, 8] = adminRepository.GetIncomeById(activity.ActivityId).ToString("0.00");
                rowCount++;
            }

            dinnerSheet.Columns.AutoFit(); // Ervoor zorgen dat de informatie in de sheet goed past in de vakjes

            return data; // Excelbestand terugsturen
        }

        private Workbook GetTalkingWorksheet(Workbook data, List<Activity> activities)
        {
            Worksheet talkingSheet = (Worksheet)data.Worksheets.Add(); // Nieuwe Sheet aan het bestand toevoegen
            talkingSheet.Name = "Talking Haarlem"; // Naam aan de sheet geven

            var heading = talkingSheet.Range[talkingSheet.Cells[1, 1], talkingSheet.Cells[1, 8]]; // De headers van het document selecteren
            heading.Interior.Color = Color.Red; // Rode achtergrondkleur aan de header geven
            heading.Font.Color = Color.White; // Witte tekstkleur aan de header geven

            // Heading text neerzetten
            talkingSheet.Cells[1, 1] = "Name";
            talkingSheet.Cells[1, 2] = "Date";
            talkingSheet.Cells[1, 3] = "Time";
            talkingSheet.Cells[1, 4] = "Total Seats";
            talkingSheet.Cells[1, 5] = "Tickets Sold";
            talkingSheet.Cells[1, 6] = "Available Tickets";
            talkingSheet.Cells[1, 7] = "Price";
            talkingSheet.Cells[1, 8] = "Income";

            int rowCount = 2; // int om aan te geven vanaf welke rij de data moet beginnen

            // Data toevoegen aan de sheet
            foreach (Talking activity in activities.OfType<Talking>())
            {
                talkingSheet.Cells[rowCount, 1] = activity.Talk.Naam;
                talkingSheet.Cells[rowCount, 2] = activity.StartSession.ToString("dddd dd-MM-yyyy");
                talkingSheet.Cells[rowCount, 3] = activity.StartSession.ToString("H:mm") + " - " + activity.EndSession.ToString("H:mm");
                talkingSheet.Cells[rowCount, 4] = activity.TotalTickets;
                talkingSheet.Cells[rowCount, 5] = activity.BoughtTickets;
                talkingSheet.Cells[rowCount, 6] = activity.TotalTickets - activity.BoughtTickets;
                talkingSheet.Cells[rowCount, 7] = activity.Price;
                talkingSheet.Cells[rowCount, 8] = adminRepository.GetIncomeById(activity.ActivityId).ToString("0.00");
                rowCount++;
            }

            talkingSheet.Columns.AutoFit(); // Ervoor zorgen dat de informatie in de sheet goed past in de vakjes

            return data; // Excelbestand terugsturen
        }

        private Workbook GetHistoricWorksheet(Workbook data, List<Activity> activities)
        {
            Worksheet historicSheet = (Worksheet)data.Worksheets.Add(); // Nieuwe Sheet aan het bestand toevoegen
            historicSheet.Name = "Historic Haarlem"; // Naam aan de sheet geven

            var heading = historicSheet.Range[historicSheet.Cells[1, 1], historicSheet.Cells[1, 10]]; // De headers van het document selecteren
            heading.Interior.Color = Color.Red; // Rode achtergrondkleur aan de header geven
            heading.Font.Color = Color.White; // Witte tekstkleur aan de header geven

            // Heading text neerzetten
            historicSheet.Cells[1, 1] = "Date";
            historicSheet.Cells[1, 2] = "Time";
            historicSheet.Cells[1, 3] = "Guide";
            historicSheet.Cells[1, 4] = "Language";
            historicSheet.Cells[1, 5] = "Total Seats";
            historicSheet.Cells[1, 6] = "Tickets Sold";
            historicSheet.Cells[1, 7] = "Available Tickets";
            historicSheet.Cells[1, 8] = "Price";
            historicSheet.Cells[1, 9] = "Group Price";
            historicSheet.Cells[1, 10] = "Income";

            int rowCount = 2; // int om aan te geven vanaf welke rij de data moet beginnen

            // Data toevoegen aan de sheet
            foreach (Historic activity in activities.OfType<Historic>())
            {
                historicSheet.Cells[rowCount, 1] = activity.StartSession.ToString("dddd dd-MM-yyyy");
                historicSheet.Cells[rowCount, 2] = activity.StartSession.ToString("H:mm") + " - " + activity.EndSession.ToString("H:mm");
                historicSheet.Cells[rowCount, 3] = activity.Guide.GuideName;
                historicSheet.Cells[rowCount, 4] = activity.Guide.LanguageName;
                historicSheet.Cells[rowCount, 5] = activity.TotalTickets;
                historicSheet.Cells[rowCount, 6] = activity.BoughtTickets;
                historicSheet.Cells[rowCount, 7] = activity.TotalTickets - activity.BoughtTickets;
                historicSheet.Cells[rowCount, 8] = activity.Price;
                historicSheet.Cells[rowCount, 9] = activity.AlternativePrice;
                historicSheet.Cells[rowCount, 10] = adminRepository.GetIncomeById(activity.ActivityId).ToString("0.00");
                rowCount++;
            }

            historicSheet.Columns.AutoFit(); // Ervoor zorgen dat de informatie in de sheet goed past in de vakjes

            return data; // Excelbestand terugsturen
        }
        #endregion

        #region Overige Functies
        private void GetCuisines(Restaurant restaurant, FormCollection collector)
        {
            // Lijst voor Cuisines ophalen.
            restaurant.Cuisines = new List<Cuisine>();
            string[] cuisineIds = collector["Cuisine"].Split(',');
            cuisineIds = cuisineIds.Distinct().ToArray();

            foreach (string cuisine in cuisineIds)
            {
                if (cuisine.Length > 0)
                    restaurant.Cuisines.Add(adminRepository.GetCuisine(Convert.ToInt32(cuisine))); // Cuisine toevoegen aan de cuisinelijst
            }
        }

        private List<DateModel> GetDateModel(List<Day> dayList)
        {
            List<DateModel> dates = new List<DateModel>();
            List<string> days = new List<string>();
            foreach (Day day in dayList) // Voor elke opgehaalde dag.
            {
                if (!days.Contains(day.Date.ToString("dddd dd-MM-yyyy")))
                {
                    days.Add(day.Date.ToString("dddd dd-MM-yyyy"));
                    dates.Add(new DateModel { DayId = day.DayId, DateDisplay = day.Date.ToString("dddd dd-MM-yyyy") }); // Nieuwe DatumModel toevoegen aan de lijst.
                }
            }

            return dates;
        }

        private string GetModelErrors()
        {
            string errors = "";
            foreach (ModelState modelstate in ViewData.ModelState.Values) // Voor elke modelstate.
            {
                foreach (ModelError error in modelstate.Errors) // Voor elke error in de modelstate
                {
                    errors += error.ErrorMessage + "<br />"; // Error Bericht toevoegen aan de string.
                }
            }

            return errors; // String met errors terugsturen.
        }

        [HttpPost]
        public ActionResult AnswerContactmessage(int id, FormCollection collector)
        {
            if (ModelState.IsValid)
            {
                ContactMessage message = adminRepository.GetContactMessage(id); // Het ingestuurde bericht uit de database halen
                message.Answer = collector["Answer"].ToString(); // Het antwoord aan het ingestuurde bericht koppelen
                message.Answered = true; // Neerzetten dat het bericht beantwoord is.
                Session["Question_Answered"] = true;
                adminRepository.UpdateContactMessage(message); // Het bericht in de Database updaten.
            }

            return RedirectToAction("AnswerContactMessage");
        }

        private bool UserAuthorized(string[] roles)
        {
            bool authorized = false;
            Account account = (Account)Session["loggedin_account"];

            // Voor elke rol die een bepaalde actie mag uitvoeren
            foreach(string role in roles)
            {
                if (role.ToLower() == account.Role.RoleName.ToLower()) // Als de ingelogde gebruiker de rol heeft
                    authorized = true;
            }

            if (!authorized) // Als de gebruiker niet Geautoriseerd is.
            {
                Session["NotAuthorized"] = true;
            }
            return authorized;
        }
        #endregion
    }
}