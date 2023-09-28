using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;
using SPLib;
using System.IO;

namespace SeniorProjectMVC.Controllers
{
    public class ImagePageController : Controller
    {
        [Route("View/{id}")]
        public IActionResult ImagePage(int id)
        {
            if (id == 0)
            {
                BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

                return View("ImageNotFoundView", model);
            }
            else
            {
                var image = SQLController.GetSingleImage(id);

                if (image.ImageID == 0)
                {
                    //Image not found
                    BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

                    return View("ImageNotFoundView", model);
                }
                else
                {
                    if (System.IO.File.Exists(image.GetPath()))
                    {
                        //Image found

                        string backUrl = HttpContext.Request.Headers.Referer;

                        var user = AccountManager.GetUserData(HttpContext);
                        if (user != null)
                        {
                            var result = SQLController.FavoriteCheck(user.UserID, id);

                            if (result.ReturnID == 1)
                            {
                                ImagePageModel model = new ImagePageModel(image, user, 
                                    "Unfavorite", backUrl);

                                return View("ImagePageView", model);
                            }
                            else
                            {
                                ImagePageModel model = new ImagePageModel(image, user, 
                                    "Favorite", backUrl);

                                return View("ImagePageView", model);
                            }
                        }
                        else
                        {
                            ImagePageModel model = new ImagePageModel(image, user, 
                                "Favorite", backUrl);

                            return View("ImagePageView", model);
                        }
                    }
                    else
                    {
                        //Image not found
                        BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

                        return View("ImageNotFoundView", model);
                    }
                }
            }
        }

        [Route("AjaxFavorite")]
        public IActionResult AjaxFavorite()
        {
            string url = HttpContext.Request.Headers.Referer;

            var urlSections = url.Split('/');

            var user = AccountManager.GetUserData(HttpContext);

            var result = SQLController.ToggleFavorite(user.UserID, urlSections[urlSections.Length - 1]);

            if (result.ReturnID == 1)
            {
                return PartialView("_ImagePartial", "Unfavorite");
            }
            else
            {
                return PartialView("_ImagePartial", "Favorite");
            }
        }

        [Route("Edit/{id}")]
        public IActionResult EditImage(int id)
        {
            if (id == 0)
            {
                BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

                return View("ImageNotFoundView", model);
            }
            else
            {
                var image = SQLController.GetSingleImage(id);

                if (image.ImageID == 0)
                {
                    //Image not found
                    BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

                    return View("ImageNotFoundView", model);
                }
                else
                {
                    if (System.IO.File.Exists(image.GetPath()))
                    {
                        //Image found

                        var user = AccountManager.GetUserData(HttpContext);
                        if (user != null)
                        {
                            var result = SQLController.FavoriteCheck(user.UserID, id);

                            if (result.ReturnID == 1)
                            {
                                ImagePageModel model = new ImagePageModel(image, user, "Unfavorite");

                                return View("ImageEditView", model);
                            }
                            else
                            {
                                ImagePageModel model = new ImagePageModel(image, user, "Favorite");

                                return View("ImageEditView", model);
                            }
                        }
                        else
                        {
                            ImagePageModel model = new ImagePageModel(image, user, "Favorite");

                            return View("ImageEditView", model);
                        }
                    }
                    else
                    {
                        //Image not found
                        BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

                        return View("ImageNotFoundView", model);
                    }
                }
            }
        }

        [Route("AjaxEdit")]
        public IActionResult AjaxEdit(FileEditFormInput input)
        {
            string url = HttpContext.Request.Headers.Referer;

            var urlSections = url.Split('/');
            int id = int.Parse(urlSections[urlSections.Length - 1]);

            var user = AccountManager.GetUserData(HttpContext);

            EditInputValidation validator = new EditInputValidation(input, id);

            validator.Validate();

            if (validator.Validated)
            {
                var result = SQLController.EditImage(user.UserID, id, input);

                if (result.ReturnID == 1)
                {
                    return PartialView("_Complete", validator);
                }
                else
                {
                    validator.ErrorMsg = result.Message;

                    return PartialView("_ValidateFail", validator);
                }
            }
            else
            {
                return PartialView("_ValidateFail", validator);
            }

        }
    }
}
