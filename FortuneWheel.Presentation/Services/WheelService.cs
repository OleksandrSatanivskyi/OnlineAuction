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

        public async Task CreateWheel(CreateWheelModel model)
        {
            if(model.WheelType == WheelType.Classic)
            {
                var wheel = new ClassicWheel(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);
                DbContext.ClassicWheels.Add(wheel);
                await DbContext.SaveChangesAsync();
            }
            else if (model.WheelType == WheelType.Point)
            {
                var wheel = new PointWheel(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);
                DbContext.PointWheels.Add(wheel);
                await DbContext.SaveChangesAsync();
            }
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
                .OrderByDescending(w => w.CreationDate)
                .ToList();

            return allWheels;
        }
    }
}
