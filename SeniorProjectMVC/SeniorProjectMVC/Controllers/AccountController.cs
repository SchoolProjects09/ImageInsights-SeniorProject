using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Classes;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("Login")]
        public IActionResult LoginGet()
        {
            if (AccountManager.IsLoggedIn(HttpContext))
            {
                //If user is already logged in
                return RedirectToAction("AccountPage");
            }
            else
            {
                return View("LoginView");
            }
        }

        [HttpPost("Login")]
        public IActionResult LoginPost(AccountFormInput input)
        {
            AccountInputValidation validator = new AccountInputValidation();

            if (validator.LoginValidate(input))
            {
                //Input valid
                var result = AccountManager.LogIn(HttpContext, new AccountData(input.UserName, input.Password));

                //If SQL login success
                if (result == "Success")
                {
                    //return RedirectToAction("Index", "Gallery");
                    validator.UserName = input.UserName;
                    return PartialView("_Login", validator);
                }
                else
                {
                    validator.LoginFail = true;
                    validator.UserName = input.UserName;
                    return PartialView("_Login", validator);
                }
            }
            else
            {
                //Return errors
                validator.UserName = input.UserName;
                return PartialView("_Login", validator);
            }
        }

        [HttpGet("Register")]
        public IActionResult RegisterGet()
        {
            if (AccountManager.IsLoggedIn(HttpContext))
            {
                //If user is already logged in
                return RedirectToAction("AccountPage");
            }
            else
            {
                return View("RegisterView");
            }
        }

        [HttpPost("Register")]
        public IActionResult RegisterPost(RegisterFormInput input)
        {
            RegisterInputValidation validator = new RegisterInputValidation(input);

            if (validator.RegisterValidate())
            {
                string result = AccountManager.Register(HttpContext, new AccountData(input.UserName, input.Password1,
                "0", input.Email));

                if (result == "Success")
                {
                    validator.ErrorMsg = "";
                    return PartialView("_Register", validator);
                }
                else
                {
                    validator.ErrorMsg = "A database error was encountered while creating your account";
                    return PartialView("_Register", validator);
                }
            }
            else
            {
                return PartialView("_Register", validator);
            }
        }

        [Route("Account")]
        public IActionResult AccountPage(string? page)
        {
            string url = $"/Account";

            if (!AccountManager.IsLoggedIn(HttpContext))
            {
                //If user is not logged in
                return RedirectToAction("LoginGet");
            }
            else
            {
                int pageNum;
                if (page == null)
                    pageNum = 1;
                else
                {
                    if (!int.TryParse(page, out pageNum))
                        pageNum = 1;
                }

                var user = AccountManager.GetUserData(HttpContext);
                var images = SQLController.GetUserImages(user.UserID, pageNum);

                AccountModel model = new AccountModel(user, images, user.UserName, url,
                    MobileCheck.IsMobileBrowser(HttpContext.Request), pageNum);

                return View("AccountPageView", model);
            }
        }

        [Route("User/{id}")]
        public IActionResult UserPage(int id, string? page)
        {
            string url = $"/User/{id}";
            int pageNum;
            if (page == null)
                pageNum = 1;
            else
            {
                if (!int.TryParse(page, out pageNum))
                    pageNum = 1;
            }

            var user = AccountManager.GetUserData(HttpContext);
            var result = SQLController.GetUserUsername(id);
            var images = SQLController.GetUserImages(id, pageNum);

            if (result.ReturnID != 0 && !string.IsNullOrWhiteSpace(result.Message))
            {
                //Account found
                AccountModel model = new AccountModel(user, images, result.Message, url,
                    MobileCheck.IsMobileBrowser(HttpContext.Request), pageNum);

                return View("PublicUserPageView", model);
            }
            else
            {
                //Account not found
                AccountModel model = new AccountModel(user, images, "Error", url,
                    MobileCheck.IsMobileBrowser(HttpContext.Request), pageNum);

                return View("UserPageErrorView", model);
            }
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            if (AccountManager.IsLoggedIn(HttpContext))
            {
                AccountManager.LogOut(HttpContext);
            }

            return RedirectToAction("Index", "Gallery");
        }

        [Route("Favorites")]
        public IActionResult Favorites(string? page)
        {
            string url = $"/Favorites";

            if (!AccountManager.IsLoggedIn(HttpContext))
            {
                //If user is not logged in
                return RedirectToAction("LoginGet");
            }
            else
            {
                int pageNum;
                if (page == null)
                    pageNum = 1;
                else
                {
                    if (!int.TryParse(page, out pageNum))
                        pageNum = 1;
                }

                var user = AccountManager.GetUserData(HttpContext);
                var images = SQLController.GetUserFavorites(user.UserID, pageNum);

                AccountModel model = new AccountModel(user, images, user.UserName, url,
                    MobileCheck.IsMobileBrowser(HttpContext.Request), pageNum);

                return View("FavoritesView", model);
            }
        }
    }
}
