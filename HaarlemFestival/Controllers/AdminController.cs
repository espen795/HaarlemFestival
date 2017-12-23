using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaarlemFestival.Models;
using System.Web.Security;
using HaarlemFestival.Repository.Admin;
using System.Text.RegularExpressions;

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

                if (account != null) // Als het account bestaat.
                {
                    FormsAuthentication.SetAuthCookie(account.Username, false);

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
            data.Dates = GetDateModel(data.Days); // DateTime omzetten naar een string om data goed weer te geven.

            string selectedEvent = this.Request.QueryString["selectedEvent"]; // Geselecteerde evenement uit de URL halen

            ViewData["Restaurants"] = data.Restaurants;
            ViewData["Guides"] = data.Guides;
            ViewData["Cuisines"] = data.Cuisines;
            ViewData["Dates"] = data.Dates;

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

        [HttpPost]
        [Authorize]
        public JsonResult AddJazz(HaarlemFestival.Models.Jazz activity, FormCollection collector)
        {
            // Standaard informatie van activity
            activity.EventType = EventType.JazzPatronaat;
            activity.BoughtTickets = 0;
            activity.AllDayPassPartout = 80;

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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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

                // Lijst met cuisines aanmaken.
                restaurant.Cuisines = new List<Cuisine>();
                string[] cuisineIds = collector["Cuisines"].Split(',');
                cuisineIds = cuisineIds.Distinct().ToArray();

                // Cuisines ophalen
                foreach (string cuisine in cuisineIds)
                    restaurant.Cuisines.Add(adminRepository.GetCuisine(Convert.ToInt32(cuisine)));

                // Restaurant toevoegen
                adminRepository.AddRestaurant(restaurant);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddGuide(Models.Guide guide, FormCollection collector)
        {
            if (ModelState.IsValid)
            {
                // Guide toevoegen.
                adminRepository.AddGuide(guide);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteEvent(int id)
        {
            // Evenement verwijderen.
            adminRepository.DeleteEvent(id);
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteRestaurant(int id)
        {
            // Restaurant verwijderen.
            adminRepository.DeleteRestaurant(id);
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteGuide(int id)
        {
            // Guide verwijderen.
            adminRepository.DeleteGuide(id);
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateEvent(Activity activity)
        {
            // Evenement updaten.
            adminRepository.UpdateEvent(activity);
            return RedirectToAction("ManageEvent", "Admin");
        }

        [Authorize]
        public ActionResult Download()
        {
            return View();
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

                default:
                    return RedirectToAction("ManageEvent", "Admin");
            }
        }
        
        [HttpPost]
        [Authorize]
        public JsonResult UpdateJazz(Models.Jazz activity, FormCollection collector)
        {
            // Prijs en Alternatieve prijs ophalen.
            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);
            activity.artist.ArtistId = activity.ArtistId;
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateRestaurant(Models.Restaurant restaurant, FormCollection collector)
        {
            //Proberen om de rating op te halen.
            if (restaurant.Rating != null)
                restaurant.Rating = restaurant.Rating + "/5";
            else
                ModelState.AddModelError("NoRating", "Please enter a valid rating");

            // Lijst voor Cuisines ophalen.
            restaurant.Cuisines = new List<Cuisine>();
            string[] cuisineIds = collector["Cuisine"].Split(',');
            cuisineIds = cuisineIds.Distinct().ToArray();

            foreach (string cuisine in cuisineIds)
            {
                if (cuisine.Length > 0)
                    restaurant.Cuisines.Add(adminRepository.GetCuisine(Convert.ToInt32(cuisine))); // Cuisine toevoegen aan de cuisinelijst
            }

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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult UpdateGuide(Models.Guide guide, FormCollection collector)
        {
            if (ModelState.IsValid)
            {
                // Guide updaten.
                adminRepository.UpdateGuide(guide);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var errors = GetModelErrors(); // ModelState errors ophalen.
            return Json(new { success = false, errorList = errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
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
                    path = System.IO.Path.Combine(Server.MapPath("~/images/jazz"), fileName);
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

        private void UpdatePrice(Activity activity, FormCollection collector)
        {
            float price;
            if (float.TryParse(collector["Price"].Replace(".", ","), out price)) // Als de prijs omgezet kan worden naar een float.
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else if(collector["Price"].ToString().Length > 0) // Als er een prijs is ingevoerd maar niet omgezet kan worden.
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

        private List<DateModel> GetDateModel(List<Day> days)
        {
            List<DateModel> dates = new List<DateModel>();
            foreach (Day day in days) // Voor elke opgehaalde dag.
            {
                dates.Add(new DateModel { DayId = day.DayId, DateDisplay = day.Date.ToString("dddd dd-MM-yy") }); // Nieuwe DatumModel toevoegen aan de lijst.
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
    }
}