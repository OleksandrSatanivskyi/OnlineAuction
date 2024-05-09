using WheelOfFortune.Domain.Segments;

namespace WheelOfFortune.Domain.WheelsOfFortune
{
    public class ClassicWheel : WheelOfFortune
    {
        public List<Segment> Segments { get; set; }

        public ClassicWheel(Guid id, string title, DateTime creationDate, Guid userId, List<Segment> segments) : base(id, title, creationDate, userId)
        {
            Segments = segments;
        }

        public ClassicWheel(Guid id, string title, DateTime creationDate, Guid userId) : base(id, title, creationDate, userId)
        {
            Segments = new List<Segment>();
        }

        public ClassicWheel() { }
    }
}
