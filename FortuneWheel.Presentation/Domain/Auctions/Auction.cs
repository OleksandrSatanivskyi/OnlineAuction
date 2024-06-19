using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuc.Domain.Auctions
{
    public class Auction
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> RemainingOptions { get; set; }

        public Auction(Guid id, string title, DateTime creationDate, Guid userId)
        {
            Id = id;
            Title = title;
            CreationDate = creationDate;
            UserId = userId;
            RemainingOptions = new List<Guid>();
        }

        public Auction() { }
    }

}
