using FortuneWheel.Data.DbContexts;
using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Services
{
    public class WheelService : IWheelService
    {
        private readonly IDbContext DbContext;

        public WheelService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IEnumerable<WheelOfFortune>> GetUserWheels(Guid userId)
        {
            var classicWheels = DbContext.ClassicWheels.Where(w => w.UserId == userId)
                .ToArray()
                .Select(c => new WheelOfFortune(c.Id, c.Title, c.CreationDate, c.UserId))
                .ToArray();
            var pointWheels = DbContext.PointWheels.Where(w => w.UserId == userId)
                .ToArray()
                .Select(c => new WheelOfFortune(c.Id, c.Title, c.CreationDate, c.UserId))
                .ToArray();

            var allWheels = classicWheels
                .Concat(pointWheels)
                .ToArray()
                .OrderBy(w => w.CreationDate);

            return allWheels;
        }
    }
}
