using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC
{
    public class ImagePartialModel
    {
        public List<GalleryImage> Images { get; set; }
        public string URL { get; set; }
        public int Page { get; set; }
        public bool IsMobile { get; set; }

        public ImagePartialModel(List<GalleryImage> images, string url, int page, bool isMobile)
        {
            Images = images;
            URL = url;
            IsMobile = isMobile;
            Page = page;
        }
    }
}
