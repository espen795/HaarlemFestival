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

            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

            // Standaard informatie van activity
            activity.AllDayPassPartout = 80;

            if (ModelState.IsValid)
            {
                try {
                    activity.artist.ArtistImage = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    UploadImage(Request.Files[0], "Jazz");
                }
                catch (Exception) { }

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = DateTime.Parse(collector["Date"] + " " + collector["EndSession"]);

                adminRepository.AddEvent(activity);
            }

            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddRestaurant(Models.Restaurant restaurant, FormCollection collector)
        {
            restaurant.Rating = restaurant.Rating + "/5";
            if (ModelState.IsValid)
            {
                try {
                    restaurant.FoodIMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    UploadImage(Request.Files[0], "Restaurant");
                }
                catch(Exception) { }

                try {
                    restaurant.LocationIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    UploadImage(Request.Files[1], "Restaurant");
                }
                catch (Exception) { }

                restaurant.Cuisines = new List<Cuisine>();
                restaurant.Cuisines.Add(adminRepository.GetCuisine(Convert.ToInt32(collector["Cuisine1"])));
                restaurant.Cuisines.Add(adminRepository.GetCuisine(Convert.ToInt32(collector["Cuisine2"])));
                restaurant.Cuisines.Add(adminRepository.GetCuisine(Convert.ToInt32(collector["Cuisine3"])));
                adminRepository.AddRestaurant(restaurant);
            }

            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddGuide(Models.Guide guide, FormCollection collector)
        {
            if(ModelState.IsValid)
            {
                adminRepository.AddGuide(guide);
            }

            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddDinner(Models.Dinner activity, FormCollection collector)
        {
            activity.EventType = EventType.DinnerInHaarlem;

            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

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
            activity.AlternativePrice = null;
            UpdatePrice(activity, collector);

            if (ModelState.IsValid)
            {
                try
                {
                    activity.Talk.Person1IMG = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    UploadImage(Request.Files[0], "Restaurant");
                }
                catch (Exception) { }

                try
                {
                    activity.Talk.Person1AltIMG = System.IO.Path.GetFileName(Request.Files[1].FileName);
                    UploadImage(Request.Files[1], "Restaurant");
                }
                catch (Exception) { }

                try
                {
                    activity.Talk.Person2IMG = System.IO.Path.GetFileName(Request.Files[2].FileName);
                    UploadImage(Request.Files[2], "Restaurant");
                }
                catch (Exception) { }

                try
                {
                    activity.Talk.Person2AltIMG = System.IO.Path.GetFileName(Request.Files[3].FileName);
                    UploadImage(Request.Files[3], "Restaurant");
                }
                catch (Exception) { }

                activity.StartSession = DateTime.Parse(collector["Date"] + " " + collector["StartSession"]);
                activity.EndSession = DateTime.Parse(collector["Date"] + " " + collector["EndSession"]);

                adminRepository.AddEvent(activity);

            }
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddHistoric(Models.Historic activity, FormCollection collector)
        {
            activity.EventType = EventType.HistoricHaarlem;
            activity.BoughtTickets = 0;

            UpdatePrice(activity, collector);
            UpdateAlternativePrice(activity, collector);

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
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteRestaurant(int id)
        {
            adminRepository.DeleteRestaurant(id);
            return RedirectToAction("ManageEvent", "Admin");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteGuide(int id)
        {
            adminRepository.DeleteGuide(id);
            return RedirectToAction("ManageEvent", "Admin");
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
        public ActionResult _JazzPartial(int? id)
        {
            ViewData["Dates"] = adminRepository.GetDates();

            Jazz jazz;
            if(id != null)
            {
                jazz = adminRepository.GetActivity((int)id) as Jazz;
            }
            else
            {
                jazz = new Jazz();
            }

            return PartialView(jazz);
        }

        [Authorize]
        public ActionResult _RestaurantPartial(int? id)
        {
            ViewData["Cuisines"] = adminRepository.GetCuisines();
            ViewData["Dates"] = adminRepository.GetDates();

            Restaurant restaurant;
            if(id != null)
            {
                restaurant = adminRepository.GetRestaurant((int)id);
                restaurant.Rating = restaurant.Rating.Substring(0, 1);
            }
            else
            {
                restaurant = new Restaurant();
            }

            return PartialView(restaurant);
        }

        [Authorize]
        public ActionResult _DinnerPartial(int? id)
        {
            ViewData["Restaurants"] = adminRepository.GetRestaurants();
            ViewData["Dates"] = adminRepository.GetDates();

            Dinner dinner;
            if (id != null)
            {
                dinner = adminRepository.GetActivity((int)id) as Dinner;
            }
            else
            {
                dinner = new Dinner();
            }

            return PartialView(dinner);
        }

        [Authorize]
        public ActionResult _TalkingPartial(int? id)
        {
            Talking talking;
            if (id != null)
            {
                talking = adminRepository.GetActivity((int)id) as Talking;
            }
            else
            {
                talking = new Talking();
            }

            return PartialView(talking);
        }

        [Authorize]
        public ActionResult _GuidePartial(int? id)
        {
            Guide guide;
            if(id != null)
            {
                guide = adminRepository.GetGuide((int)id);
            }
            else
            {
                guide = new Guide();
            }

            return PartialView(guide);
        }

        [Authorize]
        public ActionResult _HistoricPartial(int? id)
        {
            ViewData["Guides"] = adminRepository.GetGuides();

            Historic historic;
            if (id != null)
            {
                historic = adminRepository.GetActivity((int)id) as Historic;
            }
            else
            {
                historic = new Historic();
            }

            return PartialView(historic);
        }



        public ActionResult _UpdateData(int id)
        {
            string type = this.Request["type"].ToLower();

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



        // Methoden voor meergebruikte functies.
        private void UploadImage(HttpPostedFileBase file, string type)
        {
            string fileName = System.IO.Path.GetFileName(file.FileName);
            string path = "";
            switch (type)
            {
                case "Jazz":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/jazz"), fileName);
                    break;
                case "Restaurant":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/dinner"), fileName);
                    break;
                case "Dinner":
                    path = System.IO.Path.Combine(Server.MapPath("~/images/dinner"), fileName);
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
            file.SaveAs(path);
        }

        private void UpdatePrice(Activity activity, FormCollection collector)
        {
            float price;
            if (float.TryParse(collector["Price"].Replace(".", ","), out price))
            {
                ModelState["Price"].Errors.Clear();
                activity.Price = price;
            }
            else
                ModelState.AddModelError("InvalidPrice", "Please enter a valid price");
        }

        private void UpdateAlternativePrice(Activity activity, FormCollection collector)
        {
            float alternativePrice;
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
        }
    }
}