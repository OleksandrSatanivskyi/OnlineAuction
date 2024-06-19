using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class CreateAuctionModel
    {
        public string Title { get; set; }
        public Guid? UserId { get; set; }
        public AuctionType Type { get; set; }
    }
}
