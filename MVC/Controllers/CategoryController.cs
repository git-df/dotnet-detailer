using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Index()
        {
            var data = await _categoryService.GetCategoryList();

            if (data.Success && data.Data != null)
            {
                return View(data.Data);
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> AddCategory(CategoryAddModel category)
        {
            await _categoryService.AddCategory(category);

            return RedirectToAction("Index", "Category");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Active([FromRoute] int id)
        {
            var data = await _categoryService.Active(id);

            return RedirectToAction("Index", "Category");
        }

        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> DeActive([FromRoute] int id)
        {
            var data = await _categoryService.DeActive(id);

            return RedirectToAction("Index", "Category");
        }
    }
}
