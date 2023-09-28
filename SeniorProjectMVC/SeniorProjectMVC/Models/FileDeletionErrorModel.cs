using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
	public class FileDeletionErrorModel
	{
        public AccountData? Data { get; set; }
        public int ImageId { get; set; }
		public string ErrorMsg { get; set; }

		public FileDeletionErrorModel(AccountData? data, int imageId, string errorMsg)
		{
			Data = data;
			ImageId = imageId;
			ErrorMsg = errorMsg;
		}
	}
}
