using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
	public class ValidateErrorModel
	{
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool NameFail { get; set; }
        public string? FileErrorMsg { get; set; }
        public bool DescriptionFail { get; set; }
        public AccountData? Data { get; set; }

        public ValidateErrorModel(string? name, string? description, bool nameFail, string? fileErrorMsg, bool descriptionFail, AccountData? data)
        {
            Name = name;
            Description = description;
            NameFail = nameFail;
            FileErrorMsg = fileErrorMsg;
            DescriptionFail = descriptionFail;
            Data = data;
        }
    }
}
