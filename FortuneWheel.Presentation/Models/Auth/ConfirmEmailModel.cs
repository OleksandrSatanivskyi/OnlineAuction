using System.ComponentModel.DataAnnotations;

namespace WheelOfFortune.Models.Auth
{
    public class ConfirmEmailModel
    {
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Invalid code format. The code should consist of 5 digits.")]
        public string Code { get; set; }
        public string? Email { get; set; }
    }
}
