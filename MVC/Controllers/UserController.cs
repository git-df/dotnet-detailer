using Data.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAccessService _accessService;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IAccessService accessService, IUserService userService)
        {
            _logger = logger;
            _accessService = accessService;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await GetUser();

            if (user.Success)
            {
                return View(user.Data);
            }
            else
            {
                return RedirectToAction("SignIn", "Access");
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await GetUser();

            if (user.Success)
            {
                var userEdit = new UserEditModel()
                {
                    Id = user.Data.Id,
                    FirstName = user.Data.FirstName,
                    LastName = user.Data.LastName
                };

                return View(userEdit);
            }
            else
            {
                return RedirectToAction("SignIn", "Access");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel userEdit)
        {
            if (!ModelState.IsValid) return View();

            var data = await _userService.EditUser(userEdit);

            if (data.Success)
            {
                return RedirectToAction("Index", "User");
            }

            ViewData["Message"] = data.Message;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PasswordChange()
        {
            var user = await GetUser();

            if (user.Success)
            {
                var userPasswordChange = new UserPasswordChangeModel()
                {
                    Id = user.Data.Id,
                    Email = user.Data.Email
                };

                return View(userPasswordChange);
            }
            else
            {
                return RedirectToAction("SignIn", "Access");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeModel userPasswordChange)
        {
            if (!ModelState.IsValid) return View();

            var data = await _accessService.PasswordChange(userPasswordChange);

            if (data.Success)
            {
                return RedirectToAction("Index", "User");
            }

            ViewData["Message"] = data.Message;
            return View();
        }



        private async Task<BaseResponse<UserInfoModel>> GetUser()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            var userid = Int32.Parse(claimsPrincipal.FindFirstValue("UserId"));
            var user = await _accessService.GetUserInfo(userid);
            return user;
        }
    }
}
