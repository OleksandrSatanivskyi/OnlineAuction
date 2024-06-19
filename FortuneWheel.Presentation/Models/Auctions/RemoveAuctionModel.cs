using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class RemoveAuctionModel
    {
        public Guid Id { get; set; }
        public AuctionType Type { get; set; }
    }
}
