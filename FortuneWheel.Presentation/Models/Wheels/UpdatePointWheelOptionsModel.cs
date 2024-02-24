using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Models.Wheels
{
    public class UpdatePointWheelOptionsModel
    {
        public Guid WheelId { get; set; }
        public List<PointSegment> Segments { get; set; }
    }
}
