using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;
using SPLib;

namespace SeniorProjectMVC.Controllers
{
	public class SearchController : Controller
	{
        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("Search")]
        public IActionResult Index(string? search, string? page)
		{
            string url = "/Search";

            int pageNum;
            if (page == null || page == "0")
            {
                pageNum = 1;
            }
            else
            {
                if (!int.TryParse(page, out pageNum))
                    pageNum = 1;
            }

            if (String.IsNullOrWhiteSpace(search))
            {
                //If search is null, return base gallery view
                SearchModel model = new SearchModel(SQLController.GetAllImages(pageNum), url, "", pageNum,
                    AccountManager.GetUserData(HttpContext), 
                    MobileCheck.IsMobileBrowser(HttpContext.Request));

                return View("SearchView", model);
            }
            else
            {
                //Search is not null
                
                var results = SQLController.SearchImages(pageNum, search);
                //var results = Helper.GetFiles("/wwwroot/Images/", search);

                if (results.Count > 0)
                {
                    //Results found
                    return View("SearchView", new SearchModel(results, url, search, pageNum,
                        AccountManager.GetUserData(HttpContext),
                        MobileCheck.IsMobileBrowser(HttpContext.Request)));
                }
                else
                {
                    //No results found
                    return View("SearchView", new SearchModel(results, url, search, pageNum,
                        AccountManager.GetUserData(HttpContext),
                        MobileCheck.IsMobileBrowser(HttpContext.Request),
                        "No results found! Did you type that in correctly?"));
                }
            }
        }
    }
}
