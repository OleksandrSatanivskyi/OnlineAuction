using OnlineAuc.Domain.Auctions;

namespace OnlineAuc.Models.Auctions
{
    public class AuctionItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int ElementCount { get; set; }
        public Guid UserId { get; set; }
        public AuctionType Type { get; set; }

        public AuctionItem(Guid id, string title, DateTime creationDate, Guid userId, int elementCount, AuctionType wheelType)
        {
            Id = id;
            Title = title;
            CreationDate = creationDate;
            UserId = userId;
            ElementCount = elementCount;
            Type = wheelType;
        }

        public AuctionItem()
        {
            
        }
    }
}
