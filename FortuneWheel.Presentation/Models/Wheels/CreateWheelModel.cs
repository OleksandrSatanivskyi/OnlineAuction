using FortuneWheel.Domain.WheelsOfFortune;
using System.ComponentModel.DataAnnotations;

namespace FortuneWheel.Models.Wheels
{
    public class CreateWheelModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
        [MaxLength(40, ErrorMessage = "TTitle must not exceed 40 characters.")]
        public string Title { get; set; }
        public Guid? UserId { get; set; }
        [Required(ErrorMessage = "Wheel type is required.")]
        public WheelType Type { get; set; }
    }
}
