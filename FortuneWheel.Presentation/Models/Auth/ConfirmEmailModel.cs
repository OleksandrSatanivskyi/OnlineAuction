using System.ComponentModel.DataAnnotations;

namespace FortuneWheel.Models.Auth
{
    public class ConfirmEmailModel
    {
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Invalid code format. The code should consist of 5 digits.")]
        public string Code { get; set; }
    }
}
