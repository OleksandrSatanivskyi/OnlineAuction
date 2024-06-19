using OnlineAuc.Domain.Segments;

namespace OnlineAuc.Models.Auctions
{
    public class UpdateClassicAuctionOptionsModel
    {
        public Guid AuctionId { get; set; }
        public List<Segment> Segments { get; set; }
    }
}
