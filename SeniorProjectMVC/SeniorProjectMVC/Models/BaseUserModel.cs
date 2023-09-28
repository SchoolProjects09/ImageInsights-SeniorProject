using SeniorProjectMVC.SQL;

namespace SeniorProjectMVC.Models
{
    public class BaseUserModel
    {
        public AccountData? Data { get; set; }

        public BaseUserModel(AccountData? data)
        {
            Data = data;
        }
    }
}
