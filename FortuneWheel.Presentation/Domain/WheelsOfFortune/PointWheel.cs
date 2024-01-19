using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Domain.WheelsOfFortune
{
    public class PointWheel : WheelOfFortune
    {
        public List<PointSegment> Segments { get; set; }

        public PointWheel(Guid id, string title, List<PointSegment> segments) : base(id, title)
        {
            Segments = segments;
        }

        public PointWheel() { }
    }
}
