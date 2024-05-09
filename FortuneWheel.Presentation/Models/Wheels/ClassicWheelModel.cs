using WheelOfFortune.Domain.WheelsOfFortune;

namespace WheelOfFortune.Models.Wheels
{
    public class ClassicWheelModel
    {
        public ClassicWheel Wheel { get; set; }
        public string Title {  get; set; }
        public string ColorHex { get; set; }

        public ClassicWheelModel()
        {
            Title = "";
            ColorHex = "";
        }
    }
}
