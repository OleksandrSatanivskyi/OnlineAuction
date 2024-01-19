using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Domain.WheelsOfFortune
{
    public class PointWheel : WheelOfFortune
    {
        public List<PointSegment> Segments { get; set; }

        public PointWheel(Guid id, string title, DateTime creationDate, Guid userId, List<PointSegment> segments) : base(id, title, creationDate, userId)
        {
            Segments = segments;
        }

        public PointWheel() { }
    }
}
