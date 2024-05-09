using WheelOfFortune.Domain.Segments;

namespace WheelOfFortune.Models.Wheels
{
    public class UpdatePointWheelOptionsModel
    {
        public Guid WheelId { get; set; }
        public List<PointSegment> Segments { get; set; }
    }
}
