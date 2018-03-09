using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanQRCodeLoginSample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScanQRCodeLoginSample.SignalRs;

namespace ScanQRCodeLoginSample.Controllers
{
    public class HomeController : Controller
    {
        private IHubContext<SignalrHubs> _hubContext;
        public HomeController(IHubContext<SignalrHubs> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            var guid = Guid.NewGuid();
            await _hubContext.Clients.All.SendAsync("request2Login", guid);
            
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
