using FortuneWheel.Domain.Segments;

namespace FortuneWheel.Domain.WheelsOfFortune
{
    public class ClassicWheel : WheelOfFortune
    {
        public List<Segment> Segments { get; set; }

        public ClassicWheel(Guid id, string title, List<Segment> segments) : base(id, title)
        {
            Segments = segments;
        }

        public ClassicWheel() { }
    }
}
