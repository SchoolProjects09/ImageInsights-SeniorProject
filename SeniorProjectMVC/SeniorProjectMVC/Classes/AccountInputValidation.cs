namespace SeniorProjectMVC.Classes
{
    public class AccountInputValidation
    {
        public bool Validated { get; set; }
        public bool LoginFail { get; set; }
        public string UserNameFail { get; set; }
        public bool IsUserNameFail { get => UserNameFail != ""; }
        public string PasswordFail { get; set; }
        public bool IsPasswordFail { get => PasswordFail != ""; }
        public string EmailFail { get; set; }
        public bool IsEmailFail { get => EmailFail != ""; }
        public string UserName { get; set; }

        public AccountInputValidation()
        {
            Validated = true;
            LoginFail = false;
            UserNameFail = "";
            PasswordFail = "";
            EmailFail = "";
            UserName = "";
        }

        public bool LoginValidate(AccountFormInput input)
        {
            if (String.IsNullOrWhiteSpace(input.UserName))
            {
                UserNameFail = "Please enter a username!";
                Validated = false;
            }
            else
            {
                if (input.UserName.Length > 50)
                {
                    UserNameFail = "Username is too long!";
                    Validated = false;
                }
            }
            if (String.IsNullOrWhiteSpace(input.Password))
            {
                PasswordFail = "Please enter a password!";
                Validated = false;
            }
            else
            {
                if (input.Password.Length > 50)
                {
                    PasswordFail = "Password is too long!";
                    Validated = false;
                }
            }

            return Validated;
        }
    }
}
