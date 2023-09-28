using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
	public class SearchModel
	{
        public List<GalleryImage> Images { get; set; }
        public string? ErrorMessage { get; set; }
        public string URL { get; set; }
        public string Search { get; set; }
        public int Page { get; set; }
        public AccountData? Data { get; set; }
        public bool IsMobile { get; set; }

        public SearchModel(List<GalleryImage> images, string url, string search, int page, AccountData? data, bool isMobile, string? errorMessage = null)
        {
            Images = images;
            URL = url;
            Search = search;
            ErrorMessage = errorMessage;
            Data = data;
            Page = page;
            IsMobile = isMobile;
        }
    }
}
