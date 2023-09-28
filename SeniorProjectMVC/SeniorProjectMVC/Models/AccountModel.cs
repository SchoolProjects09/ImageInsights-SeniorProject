using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
	public class AccountModel
	{
		public AccountData? Data { get; set; }
        public List<GalleryImage> Images { get; set; }
		public string OwnerName { get; set; }
		public string URL { get; set; }
		public bool IsMobile { get; set; }
        public int Page { get; set; }

        public AccountModel(AccountData? data, List<GalleryImage> images, string ownerName, string url, bool isMobile, int page)
		{
			Data = data;
			Images = images;
			OwnerName = ownerName;
			URL = url;
			IsMobile = isMobile;
			Page = page;
		}
    }
}
