using System.Linq;

namespace SeniorProjectMVC.Classes
{
    public class RegisterInputValidation
    {
        public bool Validated { get; set; }
        public bool LoginFail { get; set; }
        public string UserNameFail { get; set; }
        public bool IsUserNameFail { get => UserNameFail != ""; }
        public string PasswordFail { get; set; }
        public bool IsPasswordFail { get => PasswordFail != ""; }
        public string EmailFail { get; set; }
        public bool IsEmailFail { get => EmailFail != ""; }
        public string ErrorMsg { get; set; }
        public RegisterFormInput Input { get; set; }

        public RegisterInputValidation(RegisterFormInput input)
        {
            Validated = true;
            LoginFail = false;
            UserNameFail = "";
            PasswordFail = "";
            EmailFail = "";
            ErrorMsg = "";
            Input = input;
        }

        public bool RegisterValidate()
        {
            if (String.IsNullOrWhiteSpace(Input.UserName))
            {
                UserNameFail = "Please enter a valid username. Usernames must be less than 50 characters.";
                Validated = false;
            }
            else
            {
                if (Input.UserName.Length > 50)
                {
                    UserNameFail = "Please enter a valid username. Usernames must be less than 50 characters.";
                    Validated = false;
                }
            }

            if (Input.Password1 != Input.Password2)
            {
                PasswordFail = "Both passwords must match!";
                Validated = false;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Input.Password1) || String.IsNullOrWhiteSpace(Input.Password2))
                {
                    PasswordFail = "Please enter a valid password. Passwords must be less than 50 characters.";
                    Validated = false;
                }
                else
                {
                    if (Input.Password1.Length > 50 || Input.Password2.Length > 50)
                    {
                        PasswordFail = "Please enter a valid password. Passwords must be less than 50 characters.";
                        Validated = false;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(Input.Email))
            {
                EmailFail = "Please enter a valid email. Emails must be less than 50 characters.";
                Validated = false;
            }
            else
            {
                if (!(Input.Email.Contains('@') && Input.Email.Contains('.')))
                {
                    EmailFail = "Please enter a valid email. Emails must be less than 50 characters.";
                    Validated = false;
                }
            }

            return Validated;
        }
    }
}
