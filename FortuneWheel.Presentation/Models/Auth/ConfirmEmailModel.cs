using System.ComponentModel.DataAnnotations;

namespace OnlineAuc.Models.Auth
{
    public class ConfirmEmailModel
    {
        [RegularExpression("^[0-9]{5}$")]
        public string Code { get; set; }
        public string? Email { get; set; }
    }
}
