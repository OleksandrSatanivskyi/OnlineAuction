using FortuneWheel.Data.DbContexts;
using FortuneWheel.Domain.WheelsOfFortune;
using FortuneWheel.Exceptions;
using FortuneWheel.Models.Wheels;
using Microsoft.EntityFrameworkCore;

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
            switch (model.Type)
            {
                case WheelType.Classic:
                    var classicWheel = new ClassicWheel(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);
                    DbContext.ClassicWheels.Add(classicWheel);
                    break;

                case WheelType.Point:
                    var pointWheel = new PointWheel(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);
                    DbContext.PointWheels.Add(pointWheel);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }

            await DbContext.SaveChangesAsync();

        }

        public async Task<List<WheelItem>> GetAll(Guid userId)
        {
            var classicWheels = DbContext.ClassicWheels.Where(w => w.UserId == userId)
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, WheelType.Classic))
                .ToArray();

            var pointWheels = DbContext.PointWheels.Where(w => w.UserId == userId)
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, WheelType.Point))
                .ToArray();

            var allWheels = classicWheels
                .Concat(pointWheels)
                .OrderByDescending(w => w.CreationDate)
                .ToList();

            return allWheels;
        }

        public async Task<ClassicWheel> GetClassicWheel(Guid id)
        {
            var wheel  = await DbContext.ClassicWheels.FirstOrDefaultAsync(w => w.Id == id);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {id} not found.");
            
            return wheel;
        }

        public async Task<PointWheel> GetPointWheel(Guid id)
        {
            var wheel = await DbContext.PointWheels.FirstOrDefaultAsync(w => w.Id == id);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {id} not found.");

            return wheel;
        }

        public async Task Remove(Guid id, WheelType type)
        {
            switch (type)
            {
                case WheelType.Classic:
                    var classicWheel = await DbContext.ClassicWheels.FirstOrDefaultAsync(w => w.Id == id);
                    DbContext.ClassicWheels.Remove(classicWheel);
                    break;

                case WheelType.Point:
                    var pointWheel = await DbContext.PointWheels.FirstOrDefaultAsync(w => w.Id == id);
                    DbContext.PointWheels.Remove(pointWheel);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
