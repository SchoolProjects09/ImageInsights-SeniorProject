using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;
using SPLib;

namespace SeniorProjectMVC.Controllers
{
	public class FileDeletionController : Controller
	{
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
		{
            if (AccountManager.IsLoggedIn(HttpContext))
			{
                var user = AccountManager.GetUserData(HttpContext);
                
                var image = SQLController.DeleteImage(user.UserID, id);

                if (image.ImageID != 0)
                {
                    Helper.DeleteImage(image.ImageID + image.Extension);

                    BaseUserModel model = new BaseUserModel(user);

                    return View("FileDeletionView", model);
                }
                else
                {
                    if (image.Description == "Access Denied")
                    {
                        FileDeletionErrorModel model = new FileDeletionErrorModel(user, id,
                            "You do not have permission to delete " + image.Name + image.Extension);

                        return View("FileDeletionErrorView", model);
                    }
                    else
                    {
                        FileDeletionErrorModel model = new FileDeletionErrorModel(user, id,
                            "The requested image could not be found");

                        return View("FileDeletionErrorView", model);
                    }
                }
            }
            else
            {
                //Not logged in
                FileDeletionErrorModel model = new FileDeletionErrorModel(null, id,
                    "You need to be logged in to delete images");

                return View("FileDeletionErrorView", model);
            }
		}
	}
}
