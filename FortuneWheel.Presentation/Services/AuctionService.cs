using OnlineAuc.Data.DbContexts;
using OnlineAuc.Domain.Segments;
using OnlineAuc.Domain.Auctions;
using OnlineAuc.Exceptions;
using OnlineAuc.Models.Auctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace OnlineAuc.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IDbContext DbContext;

        public AuctionService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddPointSegment(Guid wheelId, string title, uint points, string colorHex = "")
        {
            var wheel = DbContext.PointWheels.Include(w => w.Segments).FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

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

        public async Task Create(CreateAuctionModel model)
        {
            switch (model.Type)
            {
                case AuctionType.Classic:
                    var classicAuction = new ClassicAuction(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);
                    DbContext.ClassicWheels.Add(classicAuction);
                    break;

                case AuctionType.Point:
                    var pointAuction = new PointAuction(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);
                    DbContext.PointWheels.Add(pointAuction);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of auction.");
            }

            await DbContext.SaveChangesAsync();

        }

        public async Task<List<AuctionItem>> GetAll(Guid userId)
        {
            var classicWheels = DbContext.ClassicWheels.Where(w => w.UserId == userId)
                .Select(c => new AuctionItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, AuctionType.Classic))
                .ToArray();

            var pointWheels = DbContext.PointWheels.Where(w => w.UserId == userId)
                .Select(c => new AuctionItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, AuctionType.Point))
                .ToArray();

            var allWheels = classicWheels
                .Concat(pointWheels)
                .OrderByDescending(w => w.CreationDate)
                .ToList();

            return allWheels;
        }

        public async Task<ClassicAuction> GetClassic(Guid id)
        {
            var wheel  = await DbContext.ClassicWheels.Include(w => w.Segments)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");
            
            return wheel;
        }

        public async Task<PointAuction> GetPoint(Guid id)
        {
            var wheel = await DbContext.PointWheels.Include(w => w.Segments)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            return wheel;
        }

        public async Task Remove(Guid id, AuctionType type)
        {
            switch (type)
            {
                case AuctionType.Classic:
                    var classicWheel = await DbContext.ClassicWheels.FirstOrDefaultAsync(w => w.Id == id);
                    DbContext.ClassicWheels.Remove(classicWheel);
                    break;

                case AuctionType.Point:
                    var pointWheel = await DbContext.PointWheels.FirstOrDefaultAsync(w => w.Id == id);
                    DbContext.PointWheels.Remove(pointWheel);
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of auction.");
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task DeletePointSegment(Guid segmentId)
        {
            var segment = await DbContext.PointSegments.FirstOrDefaultAsync(s => s.Id == segmentId);

            if (segment == null)
                throw new NotFoundException($"Segment was not found.");

            DbContext.PointSegments.Remove(segment);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdatePointWheelOptions(UpdatePointAuctionOptionsModel model)
        {
            var wheel = await DbContext.PointWheels.FirstOrDefaultAsync(w => w.Id == model.AuctionId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = await DbContext.PointSegments.FirstOrDefaultAsync(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment was not found.");

                DbContext.PointSegments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);

                await DbContext.SaveChangesAsync();
            }
        }

        public async Task AddClassicSegment(Guid wheelId, string title, string colorHex)
        {
            var wheel = DbContext.ClassicWheels.Include(w => w.Segments).FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Auction was found.");

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

        public async Task DeleteClassicSegment(Guid id)
        {
            var segment = await DbContext.Segments.FirstOrDefaultAsync(s => s.Id == id);

            if (segment == null)
                throw new NotFoundException($"Segment was not found.");

            DbContext.Segments.Remove(segment);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateClassicWheelOptions(UpdateClassicAuctionOptionsModel model)
        {
            var wheel = await DbContext.ClassicWheels.FirstOrDefaultAsync(w => w.Id == model.AuctionId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = await DbContext.Segments.FirstOrDefaultAsync(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment was not found.");

                DbContext.Segments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);

                await DbContext.SaveChangesAsync();
            }
        }
    }
}
