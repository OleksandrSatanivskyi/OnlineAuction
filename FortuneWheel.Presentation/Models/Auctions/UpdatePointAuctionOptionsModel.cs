using OnlineAuc.Domain.Segments;

namespace OnlineAuc.Models.Auctions
{
    public class UpdatePointAuctionOptionsModel
    {
        public Guid AuctionId { get; set; }
        public List<PointSegment> Segments { get; set; }
    }
}
