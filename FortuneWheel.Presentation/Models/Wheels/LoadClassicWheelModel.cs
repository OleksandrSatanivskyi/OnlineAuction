using Microsoft.Identity.Client;
using WheelOfFortune.Domain.Segments;
using WheelOfFortune.Domain.WheelsOfFortune;

namespace WheelOfFortune.Models.Wheels
{
    public class LoadClassicWheelModel
    {
        public Guid WheelId { get; set; }
        public List<Segment> RemainingOptions { get; set; }

        public LoadClassicWheelModel()
        {

        }
    }
}