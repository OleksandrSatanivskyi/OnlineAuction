using System.ComponentModel.DataAnnotations;

namespace WheelOfFortune.Models.Auth
{
    public class SignUpModel
    {
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "Password should contain at least one lowercase letter, one uppercase letter, and one digit")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Password length must be between 8 and 64 characters")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
    }
}
