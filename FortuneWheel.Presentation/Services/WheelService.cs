using WheelOfFortune.Data.DbContexts;
using WheelOfFortune.Domain.Segments;
using WheelOfFortune.Domain.WheelsOfFortune;
using WheelOfFortune.Exceptions;
using WheelOfFortune.Models.Wheels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WheelOfFortune.Services
{
    public class WheelService : IWheelService
    {
        private readonly IDbContext DbContext;

        public WheelService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddPointSegment(Guid wheelId, string title, uint points, string colorHex = "")
        {
            var wheel = DbContext.PointWheels.Include(w => w.Segments).FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {wheelId} not found.");

            if (colorHex.IsNullOrEmpty())
                colorHex = GenerateRandomHexColor();

            var segment = new PointSegment
            {
                Id = Guid.NewGuid(),
                Title = title,
                Points = points,
                ColorHex = colorHex
            };

            if (wheel.Segments == null) 
                wheel.Segments = new List<PointSegment>();

            wheel.Segments.Add(segment);
            DbContext.PointSegments.Add(segment);

            await DbContext.SaveChangesAsync();
        }

        public string GenerateRandomHexColor()
        {
            Random random = new Random();
            byte[] rgb = new byte[3];
            random.NextBytes(rgb);

            string hexColor = $"#{rgb[0]:X2}{rgb[1]:X2}{rgb[2]:X2}";

            return hexColor;
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
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, WheelType.Classic))
                .ToArray();

            var pointWheels = DbContext.PointWheels.Where(w => w.UserId == userId)
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, WheelType.Point))
                .ToArray();

            var allWheels = classicWheels
                .Concat(pointWheels)
                .OrderByDescending(w => w.CreationDate)
                .ToList();

            return allWheels;
        }

        public async Task<ClassicWheel> GetClassicWheel(Guid id)
        {
            var wheel  = await DbContext.ClassicWheels.Include(w => w.Segments)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {id} not found.");
            
            return wheel;
        }

        public async Task<PointWheel> GetPointWheel(Guid id)
        {
            var wheel = await DbContext.PointWheels.Include(w => w.Segments)
                .FirstOrDefaultAsync(w => w.Id == id);

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

        public async Task DeletePointWheelSegment(Guid segmentId)
        {
            var segment = await DbContext.PointSegments.FirstOrDefaultAsync(s => s.Id == segmentId);

            if (segment == null)
                throw new NotFoundException($"Segment with id {segmentId} not found.");

            DbContext.PointSegments.Remove(segment);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdatePointWheelOptions(UpdatePointWheelOptionsModel model)
        {
            var wheel = await DbContext.PointWheels.FirstOrDefaultAsync(w => w.Id == model.WheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {model.WheelId} not found.");

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = await DbContext.PointSegments.FirstOrDefaultAsync(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment with id {oldSegment} not found.");

                DbContext.PointSegments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);

                await DbContext.SaveChangesAsync();
            }
        }

        public async Task AddClassicSegment(Guid wheelId, string title, string colorHex)
        {
            var wheel = DbContext.ClassicWheels.Include(w => w.Segments).FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {wheelId} not found.");

            if (colorHex.IsNullOrEmpty())
                colorHex = GenerateRandomHexColor();

            var segment = new Segment
            {
                Id = Guid.NewGuid(),
                Title = title,
                ColorHex = colorHex
            };

            if (wheel.Segments == null)
                wheel.Segments = new List<Segment>();

            wheel.Segments.Add(segment);
            DbContext.Segments.Add(segment);

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteClassicWheelSegment(Guid id)
        {
            var segment = await DbContext.Segments.FirstOrDefaultAsync(s => s.Id == id);

            if (segment == null)
                throw new NotFoundException($"Segment with id {id} not found.");

            DbContext.Segments.Remove(segment);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateClassicWheelOptions(UpdateClassicWheelOptionsModel model)
        {
            var wheel = await DbContext.ClassicWheels.FirstOrDefaultAsync(w => w.Id == model.WheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {model.WheelId} not found.");

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = await DbContext.Segments.FirstOrDefaultAsync(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment with id {oldSegment} not found.");

                DbContext.Segments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);

                await DbContext.SaveChangesAsync();
            }
        }
    }
}
