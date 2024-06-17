using OnlineAuc.Domain;

namespace OnlineAuc.Domain.Auth
{
    public class Account
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? AvatarName { get; set; }
        public string? Language { get; set; }
        public bool IsDarkTheme { get; set; }

        public Account()
        {
            Language = "en-US";
            IsDarkTheme = false;
        }
    }

}
