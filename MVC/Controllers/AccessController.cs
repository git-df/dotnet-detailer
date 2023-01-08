using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using NuGet.Protocol;
using System.Linq;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<AccessController> _logger;
        private readonly IAccessService _accessService;

        public AccessController(ILogger<AccessController> logger, IAccessService accessService)
        {
            _logger = logger;
            _accessService = accessService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated) 
                 return RedirectToAction("Index", "User");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid) return View();

            var data = await _accessService.SignIn(userLoginModel);

            if(data.Success)
            {
                if(data.Data != null)
                {
                    var logUser = data.Data;

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, logUser.Email),
                        new Claim("UserId", logUser.Id.ToString()),
                        new Claim("FirstName", logUser.FirstName),
                        new Claim("LastName", logUser.LastName),
                        new Claim("Email", logUser.Email)
                    };

                    if (logUser.Employee != null)
                    {
                        claims.Add(new Claim("EmployeeId", logUser.Employee.Id.ToString()));
                        claims.Add(new Claim("IsAdmin", logUser.Employee.IsAdmin.ToString()));
                    }

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = userLoginModel.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authenticationProperties);

                    ViewData["Message"] = data.Message;
                    return RedirectToAction("Index", "User");
                }   
            }

            ViewData["Message"] = data.Message;
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "User");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid) return View();

            var data = await _accessService.SignUp(userRegisterModel);

            if (data.Success)
            {
                ViewData["Message"] = data.Message;
                return RedirectToAction("SignIn", "Access");
            }

            ViewData["Message"] = data.Message;
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            ViewData["Message"] = "Wylogowano";
            return RedirectToAction("SignIn", "Access");
        }
    }
}
