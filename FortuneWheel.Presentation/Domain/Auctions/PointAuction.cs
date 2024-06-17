using OnlineAuc.Domain.Segments;

namespace OnlineAuc.Domain.Auctions
{
    public class PointAuction : Auction
    {
        public List<PointSegment> Segments { get; set; }

        public PointAuction(Guid id, string title, DateTime creationDate, Guid userId, List<PointSegment> segments) : base(id, title, creationDate, userId)
        {
            Segments = segments;
        }

        public PointAuction(Guid id, string title, DateTime creationDate, Guid userId) : base(id, title, creationDate, userId)
        {
            Segments = new List<PointSegment>();
        }

        public PointAuction() { }
    }
}
