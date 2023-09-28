using Microsoft.EntityFrameworkCore;
using SPLib;
using System.Collections.Generic;
using System.Security.Principal;

namespace SeniorProjectMVC.SQL
{
    public static class SQLController
    {
        public static List<User> GetAllUsers()
        {
            return SQLControllerContext.GetContext().Users.FromSqlRaw("dbo.GetAllUsers").ToList();
        }

        public static List<GalleryImage> GetAllImages(int pageNum)
        {
            return SQLControllerContext.GetContext().Images.FromSqlRaw($"dbo.GetImagesPaginated {pageNum}").ToList();
        }

        public static SingleGalleryImage GetSingleImage(int ID)
        {
            return SQLControllerContext.GetContext().SingleImages.FromSqlRaw($"dbo.GetSingleImage {ID}").ToList()[0];
        }

        public static List<GalleryImage> SearchImages(int pageNum, string match)
        {
            return SQLControllerContext.GetContext().Images.FromSqlRaw($"dbo.SearchImagesPaginated '{match}', '{pageNum}'").ToList();
        }

        public static SQLResult AddImage(Controllers.FileUploadController.ImageData data, int ID)
        {
            string cmd = $"dbo.AddImage {ID}, '{data.Name}', '{data.Extension}', {data.Size}, '{data.Description}'";
            return SQLControllerContext.GetContext().Results.FromSqlRaw(cmd).ToList()[0];
        }

        public static SQLResult ValidateLogin(AccountData account)
        {
            string cmd = $"dbo.ValidateLogin {account.UserID}, '{account.Password}'";
            return SQLControllerContext.GetContext().Results.FromSqlRaw(cmd).ToList()[0];
        }

        public static SQLResult LoginAttempt(AccountData account)
        {
            string cmd = $"dbo.LoginAttempt '{account.UserName}', '{account.Password}'";
            return SQLControllerContext.GetContext().Results.FromSqlRaw(cmd).ToList()[0];
        }

        public static List<GalleryImage> GetUserImages(int ID, int pageNum)
        {
            return SQLControllerContext.GetContext().Images.FromSqlRaw($"dbo.GetUserImagesPaginated '{ID}', '{pageNum}'").ToList();
        }

        public static SQLResult GetUserUsername(int ID)
        {
            return SQLControllerContext.GetContext().Results.FromSqlRaw($"dbo.GetUsername {ID}").ToList()[0];
        }

        public static SQLResult RegisterAccount(AccountData account)
        {
            string cmd = $"dbo.AddUser '{account.UserName}', '{account.Email}', '{account.Password}'";
            return SQLControllerContext.GetContext().Results.FromSqlRaw(cmd).ToList()[0];
        }

        public static SingleGalleryImage DeleteImage(int UserID, int ImageID)
        {
            return SQLControllerContext.GetContext().SingleImages.FromSqlRaw($"dbo.DeleteImage '{UserID}', '{ImageID}'").ToList()[0];
        }

        public static SQLResult ToggleFavorite(int UserID, string ImageID)
        {
            return SQLControllerContext.GetContext().Results.FromSqlRaw($"dbo.ToggleFavorite '{UserID}', '{ImageID}'").ToList()[0];
        }

        public static SQLResult FavoriteCheck(int UserID, int ImageID)
        {
            return SQLControllerContext.GetContext().Results.FromSqlRaw($"dbo.FavoriteCheck '{UserID}', '{ImageID}'").ToList()[0];
        }

        public static List<GalleryImage> GetUserFavorites(int ID, int pageNum)
        {
            return SQLControllerContext.GetContext().Images.FromSqlRaw($"dbo.GetUserFavoritesPaginated '{ID}', '{pageNum}'").ToList();
        }

        public static SQLResult EditImage(int userID, int ImageID, 
            FileEditFormInput input)
        {
            return SQLControllerContext.GetContext().Results.FromSqlRaw(
                $"dbo.EditImage '{userID}', '{ImageID}', '{input.Name}', '{input.Description}'").ToList()[0];
        }
    }
}
