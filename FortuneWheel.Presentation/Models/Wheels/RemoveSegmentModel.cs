using WheelOfFortune.Domain.WheelsOfFortune;

namespace WheelOfFortune.Models.Wheels
{
    public class RemoveSegmentModel
    {
        public Guid Id { get; set; }
        public WheelType Type { get; set; }
    }
}
