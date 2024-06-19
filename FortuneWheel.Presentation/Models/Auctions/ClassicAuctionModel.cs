using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class ClassicAuctionModel
    {
        public ClassicAuction Auction { get; set; }
        public string Title {  get; set; }
        public string ColorHex { get; set; }

        public ClassicAuctionModel()
        {
            Title = "";
            ColorHex = "";
        }
    }
}
