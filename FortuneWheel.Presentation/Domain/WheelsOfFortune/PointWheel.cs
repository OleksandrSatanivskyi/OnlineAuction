using WheelOfFortune.Domain.Segments;

namespace WheelOfFortune.Domain.WheelsOfFortune
{
    public class PointWheel : WheelOfFortune
    {
        public List<PointSegment> Segments { get; set; }

        public PointWheel(Guid id, string title, DateTime creationDate, Guid userId, List<PointSegment> segments) : base(id, title, creationDate, userId)
        {
            Segments = segments;
        }

        public PointWheel(Guid id, string title, DateTime creationDate, Guid userId) : base(id, title, creationDate, userId)
        {
            Segments = new List<PointSegment>();
        }

        public PointWheel() { }
    }
}
