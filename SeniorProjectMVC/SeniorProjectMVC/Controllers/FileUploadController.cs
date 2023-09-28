using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.Classes;
using SeniorProjectMVC.Models;
using SeniorProjectMVC.SQL;
using SPLib;
using System.Drawing;
using System.IO;

namespace SeniorProjectMVC.Controllers
{
    //https://code-maze.com/file-upload-aspnetcore-mvc/
    public class FileUploadController : Controller
    {
        public class ImageData
        {
            public string Name { get; set; }
            public string Extension { get; set; }
            public string Description { get; set; }
            public long Size { get; set; }
            //public System.Drawing.Image Image { get; set; }

            public ImageData(string name, string extension, string description, long size)
            {
                Name = name;
                Extension = extension;
                Description = description;
                Size = size;

                //Image = image;
            }
        }

        [HttpGet("Upload")]
        public IActionResult Upload()
        {
            //Display base view
            BaseUserModel model = new BaseUserModel(AccountManager.GetUserData(HttpContext));

            if (AccountManager.IsLoggedIn(HttpContext))
            {
                return View("UploadView", model);
            }
            else
            {
                return View("NeedLoginView", model);
            }
        }

        [HttpGet("UploadComplete")]
        public IActionResult UploadComplete(string path)
        {
            //Display new view
            UploadCompleteModel model = new UploadCompleteModel(path,
                AccountManager.GetUserData(HttpContext));

            return View("UploadCompleteView", model);
        }

        [HttpPost("Upload")]
        public IActionResult UploadPost(FileUploadFormInput input)
        {
            if (!AccountManager.IsLoggedIn(HttpContext))
            {
                return PartialView("_Login");
            }
            else
            {
                if (input == null)
                {
                    ValidateErrorModel model = new ValidateErrorModel("", "",
                        false, "File is too big! Maximum file size is 10 Mb", false,
                        AccountManager.GetUserData(HttpContext));

                    return PartialView("_ValidateError", model);
                }

                //Validate input
                FileUploadValidation validator = new FileUploadValidation();
                validator.Validate(input);

                if (!validator.Validated)
                {
                    //If input validation failed
                    ValidateErrorModel model = new ValidateErrorModel(input.Name, input.Description,
                        validator.NameFail, validator.FileErrorMsg, validator.DescriptionFail,
                        AccountManager.GetUserData(HttpContext));

                    //Display base view with added error messages
                    return PartialView("_ValidateError", model);
                }
                else
                {
                    //Handles multiple files being uploaded, but only uses the first one for now

                    var formFile = input.Files[0];

                    //Get name from form input and extension from formFile
                    string fileName = input.Name;
                    string extension = Helper.GetImageType(formFile);

                    if (extension != "error")
                    {
                        //File is an image, so convert from FormFile to Image
                        var image = Helper.FormFileToImage(formFile);
                        var user = AccountManager.GetUserData(HttpContext);

                        ImageData data = new ImageData(input.Name, extension, input.Description, formFile.Length);
                        var result = SQLController.AddImage(data, user.UserID);

                        if (result.Message == "Success")
                        {
                            //Resize and save all images

                            if (!Helper.SaveAllSizes(image, result.ReturnID.ToString(), extension))
                            {
                                //If input validation failed
                                ValidateErrorModel model = new ValidateErrorModel(input.Name, input.Description,
                                    validator.NameFail, "Error! File exceeds maximum allowed size!", validator.DescriptionFail,
                                    AccountManager.GetUserData(HttpContext));

                                SQLController.DeleteImage(user.UserID, result.ReturnID);

                                //Display base view with added error messages
                                return PartialView("_ValidateError", model);
                            }
                            else
                            {
                                //Redirect to success page
                                UploadCompleteModel model = new UploadCompleteModel(result.ReturnID.ToString(),
                                    AccountManager.GetUserData(HttpContext));

                                return PartialView("_Complete", model);
                                //return RedirectToAction("UploadComplete", new { path = result.ReturnID });
                            }
                        }
                        else
                        {
                            ValidateErrorModel model = new ValidateErrorModel(input.Name, input.Description,
                                validator.NameFail, "Database Error Encountered When Uploading Image!", validator.DescriptionFail,
                                AccountManager.GetUserData(HttpContext));

                            return PartialView("_ValidateError", model);
                        }
                    }
                    else
                    {
                        //Display file is not image error
                        ValidateErrorModel model = new ValidateErrorModel(input.Name, input.Description,
                            validator.NameFail, "File type not recognized. Only .png and .jpg files can be uploaded.", validator.DescriptionFail,
                            AccountManager.GetUserData(HttpContext));

                        //Display base view with added error messages
                        return PartialView("_ValidateError", model);
                    }
                }
            }
        }
    }
}
