using System;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCoreApp.Models;

namespace NetCoreApp.Controllers
{
    public class ActivitiesController : Controller
    {
        public bool ErrorMessage { get; private set; }

        private NetAppContext _context;
 
        public ActivitiesController(NetAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            // Make sure user is stored in session
            int? LoginId = HttpContext.Session.GetInt32("CurrentUser");
            //If user is logged in
            if((int)LoginId > 0)
            {
                return View("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

		[HttpGet]
		[Route("Dashboard")]
		public IActionResult Dashboard()
		{
			var AreTheyLoggedIn = LoginState();
			if (AreTheyLoggedIn == true){
				// Make sure user is stored in session
				int? LoginId = HttpContext.Session.GetInt32("CurrentUser");
				//If user is logged in
				if((int)LoginId != 0)
				{
					User thisUser = GetUserInfo();
					List<Activities> AllActivities = _context.Activities.Include(a => a.Participants).ToList();
					ViewBag.UserInfo = thisUser;
					ViewBag.UserId = thisUser.UserId;
					ViewBag.AllActivities = AllActivities;

					
				}
				return View("Dashboard");
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Route("New")]
		public IActionResult New()
		{
			ViewData["Message"] = "Your application description page.";
			int? LoginId = HttpContext.Session.GetInt32("CurrentUser");
			LoginState();
			return View("NewActivity");
		}

		[HttpGet]
		[Route("DisplayActivity/{id}")]
		public IActionResult DisplayActivity(int id)
		{

			List<Activities> thisActivity = _context.Activities.Where(a => a.ActivityId == id).Include(b => b.Participants).ThenInclude(c => c.User).ToList();			
			if(thisActivity.Count == 1)
			{
				ViewBag.EventInfo = thisActivity[0];
				ViewBag.EventInfo.Description = thisActivity[0].Description;
				ViewBag.EventInfo.Participants = thisActivity[0].Participants;
				Console.WriteLine("****** FOUND the Activity ******");
				return View("DisplayActivity");
			}
			else{
				TempData["Message"] = "Unable to display this activity, double check if it is still happening";
				return RedirectToAction("Dashboard");
			}
			
		}

		[HttpPost]
		[Route("CreateActivity")]
		public IActionResult CreateActivity(ActivitiesViewModel NewActivity)
		{
			Console.WriteLine("********** Enter Create Activity Process ***********");
			// HttpContext.Session.GetInt32("CurrentEvent");

			TryValidateModel(NewActivity);

			if(ModelState.IsValid)
			{
				//Get in session UserId
				User UserInfo = GetUserInfo();
				int? CurrUserId = HttpContext.Session.GetInt32("CurrentUser");
				ViewData["UserId"] = UserInfo.UserId;
				ViewData["FirstName"] = UserInfo.FirstName;

                // string duration = NewActivity.Duration;
				// DateTime parsedDuration = DateTime.Parse(duration);
				// duration = parsedDuration.ToString();

                Activities OneActivity = new Activities
                {
					Title = NewActivity.Title,
					Date = NewActivity.Date,
					Time = NewActivity.Time,
					Description = NewActivity.Description,
					Duration = NewActivity.Duration,
					UserId = UserInfo.UserId,
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now
				};

				_context.Activities.Add(OneActivity);
				_context.SaveChanges();
				HttpContext.Session.SetInt32("CurrentEvent", OneActivity.ActivityId);
				Console.WriteLine();
				Activities EventInfo = GetEventInfo();
				// return RedirectToAction("DisplayActivity", "Activity");
				return RedirectToAction("DisplayActivity", new { id = OneActivity.ActivityId });
			}
			else
			{
				Console.WriteLine("ERROR: Activity NOT created, Fail");
				TempData["Message"] = "Something is invalid here...";
				return View("NewActivity");
			}
			
		}

		[HttpGet]
		[Route("Join/{id}")]
		public IActionResult Join(int id)
		{
			Console.WriteLine("********** Enter Add Participant ***********");
			int? CurrUserId = HttpContext.Session.GetInt32("CurrentUser");
			Participant thisParticipant = _context.Participants.Where(User => User.UserId == CurrUserId).SingleOrDefault();
            
            // if (_context.users.Where(u => u.User.UserId == CurrUserId.Email).ToList().Count() > 0)
            if (thisParticipant != null)
            {
                //you have to make custom error message it does not exist in previous logic. 
                ViewBag.err = "You're already going to that event";
                return View("Dashboard");
            }
			else
			{
				// Construct the participant object
				Participant AttendingParticipant = new Participant { 
					ActivityId = id, 
					UserId = (int)CurrUserId
				};
				
				_context.Participants.Add(AttendingParticipant);
				_context.SaveChanges();

				Console.WriteLine("********** Success ***********");
				// Console.WriteLine(AttendingParticipant);
				Console.WriteLine("**********  Person is going to the Activity ***********");
				return RedirectToAction("Dashboard");
			}
			
		}

		[HttpGet]
		[Route("Leave{id}")]
		public IActionResult Leave(int id)
		{
			int? CurrUserId = HttpContext.Session.GetInt32("CurrentUser");
			Participant thisParticipant = _context.Participants.Where(a => a.ActivityId == id && a.UserId == CurrUserId).SingleOrDefault();
			_context.Participants.Remove(thisParticipant);
			_context.SaveChanges();
			Console.WriteLine("********* You Left the Activity *********");
			return RedirectToAction("Dashboard");

		}

		[HttpGet]
		[Route("Delete/{id}")]
		public IActionResult Delete(int id)
		{
			Activities thisActivity = _context.Activities.SingleOrDefault(a => a.ActivityId == id);
			_context.Activities.Remove(thisActivity);
			_context.SaveChanges();
			Console.WriteLine("********* Activity Deleted *********");


			return RedirectToAction("Dashboard");
		}

		


        // ************ Helper functions **************
        public bool LoginState()
        {
            // convert null to 0 aka false, noone is logged in
            if (HttpContext.Session.GetInt32("CurrentUser") == null)
            {
                @ViewBag.LoginState = false;
                HttpContext.Session.SetInt32("CurrentUser", 0);
                return false;
            }
            // if login state is greater than zero a user is currently logged in
            else if(HttpContext.Session.GetInt32("CurrentUser") > 0)
            {
                @ViewBag.LoginState = true;
                return true;
            }
            // for weird senarios where you're not logged in at all
            else
            {
                @ViewBag.LoginState = false;
                return false;
            }
        }

        private User GetUserInfo()
        {
            int? LoginId = HttpContext.Session.GetInt32("CurrentUser");
            if((int)LoginId != 0)
            {
                User thisUser = _context.Users.SingleOrDefault(User => User.UserId == LoginId);
                @ViewBag.UserInfo = thisUser;
                return thisUser;
            }
            else
            {
                return null;
            }
        }

		private Activities GetEventInfo()
		{
			int? EventId = HttpContext.Session.GetInt32("CurrentEvent");
			if((int)EventId != 0)
			{
				Activities thisActivity = _context.Activities.SingleOrDefault(Event => Event.ActivityId == EventId);
				@ViewBag.EventInfo = thisActivity;
				return thisActivity;
			}
			else
			{
				return null;
			}
		}

		public bool AttendActivity()
        {
            // convert null to 0 aka false, noone is logged in
            if (@ViewBag.AttendActivity == null)
            {
                @ViewBag.AttendActivity = false;
                return false;
            }
            // if RSVP hass been added set to truw
            else if(@ViewBag.AttendActivity == true)
            {
                @ViewBag.AttendingActivity = true;
                return true;
            }
            // for weird senarios where you're not logged in at all
            else
            {
                @ViewBag.AttendActivity = false;
                return false;
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
