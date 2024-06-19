using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class RemoveSegmentModel
    {
        public Guid Id { get; set; }
        public AuctionType Type { get; set; }
    }
}
