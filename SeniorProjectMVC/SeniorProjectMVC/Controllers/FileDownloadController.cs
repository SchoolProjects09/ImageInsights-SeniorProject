using Microsoft.AspNetCore.Mvc;

namespace SeniorProjectMVC.Controllers
{
    public class FileDownloadController : Controller
    {
        [HttpGet("FileDownload")]
        public IActionResult Index(string imageName)
        {
            string extension = Path.GetExtension(imageName);

            //Check formatting
            if (extension == ".jpg")
            {
                return File("~/Images/" + imageName, "image/jpeg");
            }
            else if (extension == ".png")
            {
                return File("~/Images/" + imageName, "image/png");
            }
            else
            {
                //Redirect to error page
                return Ok(new { Error = "Internal error! The file requested could not be found!" });
            }
            
        }
    }
}
