using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ywa_tracc.Models;
using ywa_tracc.Data;
using Google.Apis.YouTube.Samples;
using Microsoft.Extensions.Configuration;
using ywa_tracc.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ywa_tracc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Select()
        {
            var lastRefresh = (from refr in _context.LastRefresh
                               select refr).FirstOrDefault();

            if (lastRefresh == null)
            {
                // no refresh date exists; fetch with fetchAll = true
                Google.Apis.YouTube.Samples.YouTubeCaller.FetchVideos(_context, true);
                var refresh = new LastRefresh();
                refresh.RefreshDate = DateTime.Now;
                _context.Add(refresh);
                _context.SaveChanges();
            } else if (lastRefresh.RefreshDate.Date < DateTime.Today) 
            {
                // refresh has happened, but not yet today... pull most recent 50 videos
                // (using fetchall=False)
                Google.Apis.YouTube.Samples.YouTubeCaller.FetchVideos(_context, false);
                lastRefresh.RefreshDate = DateTime.Now;
                _context.SaveChanges();
            }


            ViewBag.Vids = (from vid in _context.Video
                            orderby vid.PostedDate descending
                            select vid
                             );
            return View();
        }

        [Authorize]
        public IActionResult Stats()
        {
            var activity = (from ua in _context.UserActivity
                            join vid in _context.Video on ua.VideoId equals vid.ID
                            where ua.UserId == _userManager.GetUserId(HttpContext.User)
                            orderby ua.WatchDate descending
                            select new UserActivity { WatchDate = ua.WatchDate, Video = vid }
                            ).ToList() ;
            ViewBag.Activities = activity; 
            return View();
        }

        public IActionResult Watch(string id)
        {
            var viewModel = new UserActivityViewModel();
            viewModel.VideoID = id;
            viewModel.video = _context.Video.Find(id);
            return View(viewModel);
        }

        [Authorize]
        public IActionResult Record(string id)
        {
            var newRecord = new UserActivity();
            newRecord.UserId = _userManager.GetUserId(HttpContext.User);
            newRecord.VideoId = id;
            Google.Apis.YouTube.Samples.YouTubeCaller.FillDurationIfNotExists(_context, id);
            newRecord.WatchDate = System.DateTime.Now;
            _context.Add(newRecord);
            _context.SaveChanges();
            return RedirectToAction("Stats");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
