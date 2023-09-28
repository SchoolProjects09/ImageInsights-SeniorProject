using Microsoft.AspNetCore.Mvc;
using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
    public class GalleryModel
    {
        public List<GalleryImage> Images { get; set; }
        public string URL { get; set; }
        public int Page { get; set; }
        public AccountData? Data { get; set; }
        public bool IsMobile { get; set; }

        public GalleryModel(List<GalleryImage> images, string url, int page, AccountData? data, bool isMobile)
        {
            Images = images;
            URL = url;
            Data = data;
            Page = page;
            IsMobile = isMobile;
        }
    }
}
