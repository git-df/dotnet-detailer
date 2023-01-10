﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<AccessController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;

        public EmployeeController(ILogger<AccessController> logger, IUserService userService, IEmployeeService employeeService)
        {
            _logger = logger;
            _userService = userService;
            _employeeService = employeeService;
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Index()
        {
            var data = await _employeeService.GetEmployeelist();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Admins()
        {
            var data = await _employeeService.GetAdminslist();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Policy = "MustBeAdmin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeModel addEmployee)
        {
            var data = await _employeeService.GetEmployeelist();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [Authorize(Policy = "MustBeAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(AddEmployeeModel addEmployee)
        {
            var data = await _employeeService.GetEmployeelist();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Delete()
        {
            var data = await _employeeService.GetEmployeelist();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> DeleteAdmin()
        {
            var data = await _employeeService.GetEmployeelist();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }
    }
}
