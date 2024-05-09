using WheelOfFortune.Domain.Segments;
using WheelOfFortune.Domain.WheelsOfFortune;

namespace WheelOfFortune.Models.Wheels
{
    public class LoadPointWheelModel
    {
        public Guid WheelId { get; set; }
        public List<PointSegment> RemainingOptions { get; set; }
        public LoadPointWheelModel()
        {
           
        }
    }
}