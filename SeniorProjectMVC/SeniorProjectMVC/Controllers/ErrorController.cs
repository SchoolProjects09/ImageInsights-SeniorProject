using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Models;
using System.Diagnostics;

namespace SeniorProjectMVC.Controllers
{
	public class ErrorController : Controller
	{
        //https://www.youtube.com/watch?v=RSRkInBX4Sc
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
