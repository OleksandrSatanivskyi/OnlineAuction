using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Models.Wheels
{
    public class UpdatePointOptionsModel
    {
        public Guid WheelId { get; set; }
        public List<PointSegment> Segments { get; set; }
    }
}
