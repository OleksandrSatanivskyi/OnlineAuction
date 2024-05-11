using FortuneWheel.Domain;
using System.Web;

namespace FortuneWheel.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? AvatarName { get; set; }
        public Language Language { get; set; }
        public bool IsDarkTheme { get; set; }
        public IFormFile Photo { get; set; }

    }
}
