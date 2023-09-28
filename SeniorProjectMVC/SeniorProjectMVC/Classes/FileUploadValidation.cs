using SeniorProjectMVC.Classes;
using System.ComponentModel.DataAnnotations;

namespace SeniorProjectMVC
{
	public class FileUploadValidation
	{
        public bool Validated { get; set; }
		public bool NameFail { get; set; }
		public string? FileErrorMsg { get; set; }
        public bool DescriptionFail { get; set; }

		public FileUploadValidation()
		{
			Validated = true;
			NameFail = false;
			FileErrorMsg = null;
            DescriptionFail = false;
		}

		public bool Validate(FileUploadFormInput input)
		{
			if (String.IsNullOrWhiteSpace(input.Name) 
				|| 0 >= input.Name.Count() || input.Name.Count() > 50)
			{
				NameFail = true;
                Validated = false;
            }
            if (String.IsNullOrWhiteSpace(input.Description) 
				|| 0 >= input.Description.Count() || input.Description.Count() > 250)
            {
                DescriptionFail = true;
                Validated = false;
            }
            
            ValidateFile(input.Files);

            return Validated;
		}

        private void ValidateFile(List<IFormFile>? files)
        {
            if (files == null || files[0] == null)
            {
                FileErrorMsg = "Please select an image!";
                Validated = false;
                return;
            }
            else
            {
                var file = files[0];

                if (file.Length == 0)
                {
                    FileErrorMsg = "Invalid data received!";
                    Validated = false;
                    return;
                }
                if (file.Length >= 10485760)
                {
                    FileErrorMsg = "File is too big! Maximum file size is 10 Mb";
                    Validated = false;
                    return;
                }
            }
        }
    }
}
