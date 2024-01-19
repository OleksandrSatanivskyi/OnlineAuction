using System.ComponentModel.DataAnnotations;

namespace FortuneWheel.Presentation.Models.Auth
{
    public class LoginModel
    {
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,64}$", ErrorMessage = "Incorrect password")]
        public string Password { get; set; }
    }
}
