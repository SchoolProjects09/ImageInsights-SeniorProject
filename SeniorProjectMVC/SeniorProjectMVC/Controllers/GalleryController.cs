using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;
using SPLib;

namespace SeniorProjectMVC.Controllers
{
    public class GalleryController : Controller
    {
        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("Gallery")]
        public IActionResult Index(string? page)
        {
            int pageNum;
            if (page == null)
                pageNum = 1;
            else
            {
                if (!int.TryParse(page, out pageNum))
                    pageNum = 1;
            }

            var images = SQLController.GetAllImages(pageNum);

            GalleryModel model = new GalleryModel(images, "/Gallery", pageNum,
                AccountManager.GetUserData(HttpContext), 
                MobileCheck.IsMobileBrowser(HttpContext.Request));

            return View("GalleryView", model);
        }
    }
}
