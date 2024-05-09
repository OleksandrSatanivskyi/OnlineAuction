using WheelOfFortune.Domain.WheelsOfFortune;

namespace WheelOfFortune.Models.Wheels
{
    public class WheelItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid UserId { get; set; }
        public WheelType Type { get; set; }

        public WheelItem(Guid id, string title, DateTime creationDate, Guid userId, WheelType wheelType)
        {
            Id = id;
            Title = title;
            CreationDate = creationDate;
            UserId = userId;
            Type = wheelType;
        }

        public WheelItem()
        {
            
        }
    }
}
