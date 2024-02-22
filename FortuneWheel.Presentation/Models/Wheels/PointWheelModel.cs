using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Models.Wheels
{
    public class PointWheelModel
    {
        public PointWheel Wheel { get; set; }

        public string Title { get; set; }
        public string ColorHex { get; set; }
        public uint Points { get; set; }

        public PointWheelModel()
        {
            Title = "";
            ColorHex = "";
        }
    }
}
