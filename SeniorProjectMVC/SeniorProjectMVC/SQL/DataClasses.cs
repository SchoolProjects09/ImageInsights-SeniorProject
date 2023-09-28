using SPLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.ComponentModel.DataAnnotations;

namespace SeniorProjectMVC.SQL
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        //public ICollection<Image> Images { get; set; }
    }

    public class GalleryImage
    {
        [Key]
        public int ImageID { get; set; }
        public int OwnerID { get; set; }
        public string? Name { get; set; }
        public string? Extension { get; set; }
        public long ImageSize { get; set; }
        public string? Description { get; set; }

        public string GetPath()
        {
            return Helper.CWD + "/wwwroot/Images/" + ImageID + Extension;
        }
    }

    public class SingleGalleryImage
    {
        [Key]
        public int ImageID { get; set; }
        public int OwnerID { get; set; }
        public string? OwnerUsername { get; set; }
        public string? Name { get; set; }
        public string? Extension { get; set; }
        public long ImageSize { get; set; }
        public string? Description { get; set; }

        public string GetPath()
        {
            return Helper.CWD + "/wwwroot/Images/" + ImageID + Extension;
        }
    }

    public class SQLResult
    {
        [Key]
        public int ReturnID { get; set; }
        public string? Message { get; set; }
    }
}
