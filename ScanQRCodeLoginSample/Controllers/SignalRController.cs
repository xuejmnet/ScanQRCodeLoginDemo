using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ScanQRCodeLoginSample.SignalRs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ScanQRCodeLoginSample.Models;
using System.Collections.Concurrent;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanQRCodeLoginSample.Controllers
{
    public class SignalRController : Controller
    {
        public static ConcurrentDictionary<Guid, string> scanQRCodeDics = new ConcurrentDictionary<Guid, string>();

        private IHubContext<SignalrHubs> _hubContext;
        public SignalRController(IHubContext<SignalrHubs> hubContext)
        {
            _hubContext = hubContext;
        }
        //只能手机客户端发起
        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme), Route("api/[controller]/[action]")]
        public async Task<IActionResult> Send2FontRequest([FromBody]ScanQRCodeDTO qRCodeDTO)
        {
            var guid = Guid.NewGuid();
            scanQRCodeDics[guid] = qRCodeDTO.Name;
            await _hubContext.Clients.Client(qRCodeDTO.ConnectionID).SendAsync("request2Login",guid);
            return Ok();
        }

    }
}
