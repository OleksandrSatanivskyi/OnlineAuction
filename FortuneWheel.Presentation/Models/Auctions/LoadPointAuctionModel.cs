using OnlineAuc.Domain.Segments;

namespace OnlineAuc.Models.Auctions
{
    public class LoadPointAuctionModel
    {
        public Guid WheelId { get; set; }
        public List<PointSegment> RemainingOptions { get; set; }
        public LoadPointAuctionModel()
        {
           
        }
    }
}