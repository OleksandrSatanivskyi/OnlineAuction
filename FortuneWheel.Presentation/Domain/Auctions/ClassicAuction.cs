using OnlineAuc.Domain.Segments;

namespace OnlineAuc.Domain.Auctions
{
    public class ClassicAuction : Auction
    {
        public List<Segment> Segments { get; set; }

        public ClassicAuction(Guid id, string title, DateTime creationDate, Guid userId, List<Segment> segments) : base(id, title, creationDate, userId)
        {
            Segments = segments;
        }

        public ClassicAuction(Guid id, string title, DateTime creationDate, Guid userId) : base(id, title, creationDate, userId)
        {
            Segments = new List<Segment>();
        }

        public ClassicAuction() { }
    }
}
