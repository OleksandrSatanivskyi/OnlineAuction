using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune.Domain.WheelsOfFortune
{
    public class WheelOfFortune
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> RemainingOptions { get; set; }

        public WheelOfFortune(Guid id, string title, DateTime creationDate, Guid userId)
        {
            Id = id;
            Title = title;
            CreationDate = creationDate;
            UserId = userId;
            RemainingOptions = new List<Guid>();
        }

        public WheelOfFortune() { }
    }

}
