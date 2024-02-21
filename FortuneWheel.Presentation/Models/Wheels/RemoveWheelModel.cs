using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Models.Wheels
{
    public class RemoveWheelModel
    {
        public Guid Id { get; set; }
        public WheelType Type { get; set; }
    }
}
