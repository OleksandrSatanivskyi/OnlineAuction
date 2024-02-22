using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Models.Wheels
{
    public class ClassicWheelModel
    {
        public ClassicWheel Wheel { get; set; }
        public string Name {  get; set; }
        public string ColorHex { get; set; }

        public ClassicWheelModel()
        {
            Name = "";
            ColorHex = "";
        }
    }
}
