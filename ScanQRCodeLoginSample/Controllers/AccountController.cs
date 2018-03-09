using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ScanQRCodeLoginSample.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ScanQRCodeLoginSample.Controllers
{
    public class AccountController : Controller
    {
        //默认数据库用户 default database users
        public static List<LoginViewModel> _users = new List<LoginViewModel>
        {
            new LoginViewModel(){ Email="1234567@qq.com", Password="123"},

            new LoginViewModel(){ Email="12345678@qq.com", Password="123"}
        };
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ScanQRCodeLogin(string uid)
        {
            string name = string.Empty;

            if (!User.Identity.IsAuthenticated && SignalRController.scanQRCodeDics.TryGetValue(new Guid(uid), out name))
            {
                var user = AccountController._users.FirstOrDefault(o => o.Email == name);
                if (user != null)
                {
                    var claims = new Claim[] {
                        new Claim(ClaimTypes.Name,user.Email),
                        new Claim(ClaimTypes.Role,"admin")
                    };
                    var claimIdenetiy = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdenetiy));
                    SignalRController.scanQRCodeDics.TryRemove(new Guid(uid), out name);
                    
                    return Ok(new { Url = "/Home/Index" });
                }
            }

            return BadRequest();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _users.FirstOrDefault(o => o.Email == model.Email && o.Password == model.Password);
                if (user != null)
                {
                    var claims = new Claim[] {
                        new Claim(ClaimTypes.Name,user.Email),
                        new Claim(ClaimTypes.Role,"admin")
                    };
                    var claimIdenetiy = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdenetiy));
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}