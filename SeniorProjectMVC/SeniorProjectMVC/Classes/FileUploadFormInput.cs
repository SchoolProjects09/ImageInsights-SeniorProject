namespace SeniorProjectMVC.Classes
{
	public class FileUploadFormInput
	{
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
