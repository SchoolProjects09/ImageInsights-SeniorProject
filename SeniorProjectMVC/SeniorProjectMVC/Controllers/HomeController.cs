using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;
using System.Diagnostics;

namespace SeniorProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

            return View(model);
        }

        [Route("Info")]
        public IActionResult Information()
        {
            BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

            return View(model);
        }

        //Moved error handling to dedicated controller
    }
}