using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Models.Wheels
{
    public class RemoveSegmentModel
    {
        public Guid Id { get; set; }
        public WheelType Type { get; set; }
    }
}
