using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Domain.WheelsOfFortune
{
    public class ClassicWheel : WheelOfFortune
    {
        public List<Segment> Segments { get; set; }

        public ClassicWheel(Guid id, string title, DateTime creationDate, Guid userId, List<Segment> segments) : base(id, title, creationDate, userId)
        {
            Segments = segments;
        }

        public ClassicWheel() { }
    }
}
