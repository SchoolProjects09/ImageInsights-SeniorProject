using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.SQL;
using SPLib;
using System.Drawing;

namespace SeniorProjectMVC.Controllers
{
	public class DebugController : Controller
	{
        [HttpGet("Debug/Resize")]
        public IActionResult Resize()
		{
			bool admin = AdminCheck();

			if (admin)
			{
                var files = Helper.GetFiles("/wwwroot/Images/");

                foreach (var file in files)
                {
                    var image = Helper.LoadImage("/wwwroot/Images/" + file.Name);

                    string extension = file.Extension;
                    string path = "/wwwroot/Images/Resized/" + Path.GetFileNameWithoutExtension(file.Name);

                    Helper.SaveImage(Helper.CreateMobileThumbnail(image), path + "_Mthumb", extension);
                    Helper.SaveImage(Helper.CreateThumbnail(image), path + "_thumb", extension);
                    Helper.SaveImage(Helper.ResizeImage(image, 400), path + "_400", extension);
                    Helper.SaveImage(Helper.ResizeImage(image, 600), path + "_600", extension);
                    Helper.SaveImage(Helper.ResizeImage(image, 800), path + "_800", extension);
                    Helper.SaveImage(Helper.ResizeImage(image, 1000), path + "_1000", extension);
                    Helper.SaveImage(Helper.ResizeImage(image, 1200), path + "_1200", extension);

                    //Helper.SaveAllSizes(image, Path.GetFileNameWithoutExtension(file.Name), file.Extension);
                }

                return Content("Done");
            }
            else
            {
                return Content("You do not have permission to use this command");
            }
		}

        [HttpGet("Debug/Mobile")]
        public IActionResult Mobile()
        {
            if (MobileCheck.IsMobileBrowser(HttpContext.Request))
            {
                return Content("Mobile");
            }
            else
            {
                return Content("Not Mobile");
            }
        }

        private bool AdminCheck()
		{
			var user = AccountManager.GetUserData(HttpContext);

			if (user != null)
				return user.UserID == 1;
			else
				return false;
		}

    }
}
