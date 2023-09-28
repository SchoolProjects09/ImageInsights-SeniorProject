namespace SeniorProjectMVC.SQL
{
    public class AccountData
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public AccountData(string userName, string password, string userID="0", string email = "0")
        {
            UserName = userName;
            Password = password;
            UserID = int.Parse(userID);
            Email = email;
        }
    }

    //https://asp.mvc-tutorial.com/httpcontext/cookies/
    public static class AccountManager
    {
        public static bool IsLoggedIn(HttpContext context)
        {
            return (context.Request.Cookies.ContainsKey("UserID") &&
                    context.Request.Cookies.ContainsKey("UserName") &&
                    context.Request.Cookies.ContainsKey("Password"));
        }

        public static bool ValidateLogin(HttpContext context)
        {
            if (IsLoggedIn(context))
            {
                //If cookie data exists, retrieve cookie data
                AccountData? account = GetUserData(context);

                //SQL check if credentials match
                var result = SQLController.ValidateLogin(account);

                if (result.Message == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static string LogIn(HttpContext context, AccountData account)
        {
            //Make SQL Check
            var result = SQLController.LoginAttempt(account);

            if (result.Message != null)
            {
                if (result.Message == "Success")
                {
                    //Returns userID as ReturnID
                    //Save account data to cookie
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.IsEssential = true;
                    cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(1));
                    context.Response.Cookies.Append("UserID", result.ReturnID.ToString(), cookieOptions);
                    context.Response.Cookies.Append("UserName", account.UserName, cookieOptions);
                    context.Response.Cookies.Append("Password", account.Password, cookieOptions);
                }
                return result.Message;
            }
            else
            {
                return "SQL Error";
            }
        }

        public static void LogOut(HttpContext context)
        {
            if (IsLoggedIn(context))
            {
                context.Response.Cookies.Delete("UserID");
                context.Response.Cookies.Delete("UserName");
                context.Response.Cookies.Delete("Password");
            }
        }

        public static AccountData? GetUserData(HttpContext context)
        {
            if (IsLoggedIn(context))
            {
                return new AccountData(
                    context.Request.Cookies["UserName"],
                    context.Request.Cookies["Password"],
                    context.Request.Cookies["UserID"]);
            }
            else
            {
                return null;
            }
        }

        public static string Register(HttpContext context, AccountData newAccount)
        {
            var result1 = SQLController.RegisterAccount(newAccount);

            if (result1.Message == "Success")
            {
                string result2 = LogIn(context, newAccount);

                return result2;
            }
            else
            {
                return result1.Message;
            }
        }
    }
}
