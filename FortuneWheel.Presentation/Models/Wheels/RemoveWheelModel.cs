using WheelOfFortune.Domain.WheelsOfFortune;

namespace WheelOfFortune.Models.Wheels
{
    public class RemoveWheelModel
    {
        public Guid Id { get; set; }
        public WheelType Type { get; set; }
    }
}
