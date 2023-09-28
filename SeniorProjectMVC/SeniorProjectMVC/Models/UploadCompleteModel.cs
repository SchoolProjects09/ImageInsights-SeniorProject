using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
    public class UploadCompleteModel
    {
        public string Path { get; set; }
        public AccountData? Data { get; set; }

        public UploadCompleteModel(string path, AccountData? data)
        {
            Path = path;
            Data = data;
        }
    }
}
