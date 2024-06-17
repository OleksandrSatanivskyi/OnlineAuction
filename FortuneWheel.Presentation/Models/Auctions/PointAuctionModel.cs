using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class PointAuctionModel
    {
        public PointAuction Auction { get; set; }
        public string Title { get; set; }
        public string ColorHex { get; set; }
        public uint Points { get; set; }

        public PointAuctionModel()
        {
            Title = "";
            ColorHex = "";
            Points = 1;
        }
    }
}
