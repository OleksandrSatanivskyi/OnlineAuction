using Microsoft.Identity.Client;
using OnlineAuc.Domain.Segments;
using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class LoadClassicAuctionModel
    {
        public Guid WheelId { get; set; }
        public List<Segment> RemainingOptions { get; set; }

        public LoadClassicAuctionModel()
        {

        }
    }
}