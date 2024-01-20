using FortuneWheel.Data.DbContexts;
using FortuneWheel.Domain.WheelsOfFortune;
using FortuneWheel.Models.Wheels;

namespace FortuneWheel.Services
{
    public class WheelService : IWheelService
    {
        private readonly IDbContext DbContext;

        public WheelService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<WheelItem>> GetUserWheels(Guid userId)
        {
            var classicWheels = DbContext.ClassicWheels.Where(w => w.UserId == userId)
                .ToArray()
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, WheelType.Classic))
                .ToArray();
            var pointWheels = DbContext.PointWheels.Where(w => w.UserId == userId)
                .ToArray()
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, WheelType.Point))
                .ToArray();

            var allWheels = classicWheels
                .Concat(pointWheels)
                .ToArray()
                .OrderBy(w => w.CreationDate)
                .ToList();

            return allWheels;
        }
    }
}
