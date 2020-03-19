using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;

namespace BeltExam.Controllers
{
    public class ActivityController : Controller
    {
        private ActivitiesContext _context;

        public ActivityController(ActivitiesContext context)
        {
            _context = context;
        }

        // GET: /
        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                List<Activity> AllActivities = _context.Activities
                  .Include(activity => activity.Coordinator)
                  .Include(activity => activity.Participants)
                  .ThenInclude(join => join.User).ToList();
                List<Dictionary<string, object>> ActivityList = new List<Dictionary<string, object>>();
                foreach (Activity activity in AllActivities)
                {
                  bool created = false;
                  bool JOINed = false;
                  int JOINs = 0;
                  if (HttpContext.Session.GetInt32("UserId") == activity.UserId)
                  {
                    created = true;
                  }
                  foreach (var join in activity.Participants)
                  {
                    if (join.UserId == HttpContext.Session.GetInt32("UserId"))
                    {
                      JOINed = true;
                    }
                    ++JOINs;
                  }
                  Dictionary<string, object> newActivity = new Dictionary<string, object>();
                  newActivity.Add("ActivityId", activity.ActivityId);
                  newActivity.Add("ActivityName", activity.ActivityName);
                  newActivity.Add("ActivityTime", activity.ActivityTime);
                  newActivity.Add("ActivityDate", activity.ActivityDate);
                  newActivity.Add("ActivityLength", activity.ActivityLength);
                  newActivity.Add("Coordinator", activity.Coordinator);
                  newActivity.Add("Created", created);
                  newActivity.Add("JOINs", JOINs);
                  newActivity.Add("JOINed", JOINed);
                  ActivityList.Add(newActivity);
                }
                ViewBag.Activities = ActivityList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "LogReg");
            }
        }

        [HttpGet]
        [Route("delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
          Activity CancelActivity = _context.Activities.SingleOrDefault(
            w => w.UserId == (int)HttpContext.Session.GetInt32("UserId") &&
            w.ActivityId == ActivityId);
          if (CancelActivity != null)
          {
            _context.Activities.Remove(CancelActivity);
            _context.SaveChanges();
          }
          return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("Leave/{ActivityId}")]
        public IActionResult Leave(int ActivityId)
        {
          Participant leaveActivity = _context.Participants.SingleOrDefault(
            r => r.UserId == (int)HttpContext.Session.GetInt32("UserId") &&
            r.ActivityId == ActivityId);
          if (leaveActivity != null)
          {
            _context.Participants.Remove(leaveActivity);
            _context.SaveChanges();
          }
          return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("Join/{ActivityId}")]
        public IActionResult Join(int ActivityId)
        {
          Participant newJOIN = new Participant{
            UserId = (int)HttpContext.Session.GetInt32("UserId"),
            ActivityId = ActivityId
          };
          Participant existingJOIN = _context.Participants.SingleOrDefault(
            r => r.UserId == (int)HttpContext.Session.GetInt32("UserId") &&
            r.ActivityId == ActivityId);
          if (existingJOIN == null)
          {
            _context.Participants.Add(newJOIN);
            _context.SaveChanges();
          }
          return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("New")]
        public IActionResult ActivityForm()
        {
          return View("ActivityForm");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(Activity activity)
        {
          if (ModelState.IsValid)
          {
            Activity newActivity = new Activity{
              ActivityName = activity.ActivityName,
              ActivityTime = activity.ActivityTime,
              ActivityDate = activity.ActivityDate,
              ActivityLength = activity.ActivityLength,
              ActivityDescription = activity.ActivityDescription,
              CreatedAt = DateTime.UtcNow,
              UpdatedAt = DateTime.UtcNow,
              UserId = (int)HttpContext.Session.GetInt32("UserId")
            };
            _context.Activities.Add(newActivity);
            _context.SaveChanges();
            return RedirectToAction("Home");
          }
          else
          {
            return View("ActivityForm", activity);
          }
        }

        [HttpGet]
        [Route("activity/{ActivityId}")]
        public IActionResult Activity(int ActivityId)
        {
          Activity thisActivity = _context.Activities
            .Include(w => w.Participants)
            .ThenInclude(r => r.User)
            .SingleOrDefault(w => w.ActivityId == ActivityId);
          ViewBag.ThisActivity = thisActivity;
          return View("Activity");
        }
    }
}