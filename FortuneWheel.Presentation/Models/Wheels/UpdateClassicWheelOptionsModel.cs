using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Models.Wheels
{
    public class UpdateClassicWheelOptionsModel
    {
        public Guid WheelId { get; set; }
        public List<Segment> Segments { get; set; }
    }
}
