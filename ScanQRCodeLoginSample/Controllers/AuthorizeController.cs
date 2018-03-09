using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanQRCodeLoginSample.Models;
using Microsoft.Extensions.Options;
using ScanQRCodeLoginSample.Models.JWT;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanQRCodeLoginSample.Controllers
{
    public class AuthorizeController : Controller
    {
        private JwtSettings _jwtOptions;
        public AuthorizeController(IOptions<JwtSettings> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        // GET: api/<controller>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        public async Task<IActionResult> Token([FromBody]LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user=AccountController._users.FirstOrDefault(o => o.Email == viewModel.Email && o.Password == viewModel.Password);
                if(user!=null)
                    {

                    var claims = new Claim[] {
                        new Claim(ClaimTypes.Name,user.Email),
                        new Claim(ClaimTypes.Role,"admin")
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }
            return BadRequest();
        }
    }
}
