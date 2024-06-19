using OnlineAuc.Domain;
using System.Web;

namespace OnlineAuc.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? AvatarName { get; set; }
        public string Language { get; set; }
        public bool IsDarkTheme { get; set; }
        public IFormFile Photo { get; set; }

    }
}
