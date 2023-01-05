using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccessService _accessService;

        public AccessController(ILogger<HomeController> logger, IAccessService accessService)
        {
            _logger = logger;
            _accessService = accessService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser == null)
            {
                ViewData["Message"] = "Jesteś już zalogowany";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserLoginModel userLoginModel)
        {
            var data = await _accessService.LogIn(userLoginModel);

            if(data.Success)
            {
                List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, userLoginModel.Email),
                        new Claim("OtherProperties", "ExampleRole")
                    };

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
                return RedirectToAction("Index", "Home");
            }

            ViewData["Message"] = data.Message;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser == null)
            {
                ViewData["Message"] = "Jesteś już zalogowany";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            var data = await _accessService.Register(userRegisterModel);

            if (data.Success)
            {

            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            ViewData["Message"] = "Wylogowano";
            return RedirectToAction("LogIn", "Access");
        }
    }
}
