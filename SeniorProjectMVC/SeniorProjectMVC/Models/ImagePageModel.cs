using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
	public class ImagePageModel
	{
		public SingleGalleryImage Image { get; set; }
        public AccountData? Data { get; set; }
		public string Favorited { get; set; }
		public string? BackUrl { get; set; }

        public ImagePageModel(SingleGalleryImage image, AccountData? data, string favorited, string? backUrl = null)
		{
			Image = image;
			Data = data;
			Favorited = favorited;
			BackUrl = backUrl;
		}
	}
}
