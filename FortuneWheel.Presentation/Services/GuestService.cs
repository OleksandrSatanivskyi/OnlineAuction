using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OnlineAuc.Domain.Segments;
using OnlineAuc.Domain.Auctions;
using OnlineAuc.Exceptions;
using OnlineAuc.Models.Auctions;

namespace OnlineAuc.Services
{
    public class GuestService : IGuestService
    {
        private const string ClassicWheels = "ClassicWheels";
        private const string PointWheels = "PointWheels";
        private const int CookieExpirationDays = 7;

        public async Task AddClassicSegment(Guid wheelId, string title, string colorHex, HttpContext httpContext)
        {
            var wheels = await GetClassicWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            if (colorHex.IsNullOrEmpty())
                colorHex = GenerateRandomHexColor();

            wheels.Remove(wheel);

            var segment = new Segment
            {
                Id = Guid.NewGuid(),
                Title = title,
                ColorHex = colorHex
            };

            if (wheel.Segments == null)
                wheel.Segments = new List<Segment>();

            wheel.Segments.Add(segment);

            wheels.Add(wheel);

            httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }

        public string GenerateRandomHexColor()
        {
            Random random = new Random();
            byte[] rgb = new byte[3];
            random.NextBytes(rgb);

            string hexColor = $"#{rgb[0]:X2}{rgb[1]:X2}{rgb[2]:X2}";

            return hexColor;
        }

        public async Task AddPointSegment(Guid wheelId, string title, uint points, string colorHex, HttpContext httpContext)
        {
            var wheels = await GetPointWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel was not found.");


            if (colorHex.IsNullOrEmpty())
                colorHex = GenerateRandomHexColor();

            wheels.Remove(wheel);

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

            wheels.Add(wheel);

            httpContext.Response.Cookies.Append(PointWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }

        public async Task Create(CreateAuctionModel model, HttpContext httpContext)
        {
            switch (model.Type)
            {
                case AuctionType.Classic:
                    var classicWheels = await GetClassicWheels(httpContext);
                    var classicWheel = new ClassicAuction(Guid.NewGuid(), model.Title, DateTime.UtcNow, new Guid());

                    classicWheels.Add(classicWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(classicWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays) 
                    });
                    break;

                case AuctionType.Point:
                    var pointWheels = await GetPointWheels(httpContext);
                    var pointWheel = new PointAuction(Guid.NewGuid(), model.Title, DateTime.UtcNow, new Guid());

                    pointWheels.Add(pointWheel);

                    httpContext.Response.Cookies.Append(PointWheels, JsonConvert.SerializeObject(pointWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
                    });
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of auction.");
            }

        }

        public async Task DeleteClassicSegment(Guid segmentId, HttpContext httpContext)
        {
            var wheels = await GetClassicWheels(httpContext);
            var wheelContainingSegment = wheels.FirstOrDefault(w => w.Segments.Any(s => s.Id == segmentId));

            wheels.Remove(wheelContainingSegment);

            if (wheelContainingSegment != null)
            {
                var segmentToRemove = wheelContainingSegment.Segments.FirstOrDefault(s => s.Id == segmentId);
                if (segmentToRemove != null)
                {
                    wheelContainingSegment.Segments.Remove(segmentToRemove);
                }
            }
            
            wheels.Add(wheelContainingSegment);

            httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }

        public async Task DeletePointSegment(Guid segmentId, HttpContext httpContext)
        {
            var wheels = await GetPointWheels(httpContext);
            var wheelContainingSegment = wheels.FirstOrDefault(w => w.Segments.Any(s => s.Id == segmentId));

            wheels.Remove(wheelContainingSegment);

            if (wheelContainingSegment != null)
            {
                var segmentToRemove = wheelContainingSegment.Segments.FirstOrDefault(s => s.Id == segmentId);
                if (segmentToRemove != null)
                {
                    wheelContainingSegment.Segments.Remove(segmentToRemove);
                }
            }

            wheels.Add(wheelContainingSegment);

            httpContext.Response.Cookies.Append(PointWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }

        public async Task<List<AuctionItem>> GetAll(HttpContext httpContext)
        {
            var classicWheels = await GetClassicWheels(httpContext);
            var classicWheelsItems = classicWheels
                .Select(c => new AuctionItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, AuctionType.Classic))
                .ToList();

            var pointWheels = await GetPointWheels(httpContext);
            var pointWheelsItems = pointWheels
                .Select(c => new AuctionItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, AuctionType.Point))
                .ToArray();

            var allWheels = classicWheelsItems
                .Concat(pointWheelsItems)
                .OrderByDescending(w => w.CreationDate)
                .ToList();

            return allWheels;
        }

        public async Task<List<ClassicAuction>> GetClassicWheels(HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[ClassicWheels];
            List<ClassicAuction> wheels;

            if (wheelsData != null)
            {
                wheels = JsonConvert.DeserializeObject<List<ClassicAuction>>(wheelsData);
            }
            else
            {
                wheels = new List<ClassicAuction>();
            }

            return wheels;
        }

        public async Task<List<PointAuction>> GetPointWheels(HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[PointWheels];
            List<PointAuction> wheels;

            if (wheelsData != null)
            {
                wheels = JsonConvert.DeserializeObject<List<PointAuction>>(wheelsData);
            }
            else
            {
                wheels = new List<PointAuction>();
            }

            return wheels;
        }

        public async Task<ClassicAuction> GetClassic(Guid wheelId, HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[ClassicWheels];
            List<ClassicAuction> wheels;

            if (wheelsData != null)
            {
                wheels = JsonConvert.DeserializeObject<List<ClassicAuction>>(wheelsData);
            }
            else
            {
                wheels = new List<ClassicAuction>();
            }

            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            return wheel;
        }

        public async Task<PointAuction> GetPoint(Guid wheelId, HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[PointWheels];
            List<PointAuction> wheels;

            if (wheelsData != null)
            {
                wheels = JsonConvert.DeserializeObject<List<PointAuction>>(wheelsData);
            }
            else
            {
                wheels = new List<PointAuction>();
            }

            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            return wheel;
        }

        public async Task Remove(Guid wheelId, AuctionType type, HttpContext httpContext)
        {
            switch (type)
            {
                case AuctionType.Classic:
                    var classicWheels = await GetClassicWheels(httpContext);
                    var classicWheel = classicWheels.FirstOrDefault(c => c.Id == wheelId);

                    if (classicWheel == null)
                        throw new NotFoundException($"Auction was not found.");

                    classicWheels.Remove(classicWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(classicWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
                    });
                    break;

                case AuctionType.Point:
                    var pointWheels = await GetPointWheels(httpContext);
                    var pointWheel = pointWheels.FirstOrDefault(c => c.Id == wheelId);

                    if (pointWheel == null)
                        throw new NotFoundException($"Auction was not found.");

                    pointWheels.Remove(pointWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(pointWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
                    });
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of auction.");
            }
        }

        public async Task UpdateClassicWheelOptions(UpdateClassicAuctionOptionsModel model, HttpContext httpContext)
        {
            var wheels = await GetClassicWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == model.AuctionId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            wheels.Remove(wheel);

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = wheel.Segments.FirstOrDefault(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment was not found.");

                wheel.Segments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);
            }

            httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }

        public async Task UpdatePointWheelOptions(UpdatePointAuctionOptionsModel model, HttpContext httpContext)
        {
            var wheels = await GetPointWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == model.AuctionId);

            if (wheel == null)
                throw new NotFoundException($"Auction was not found.");

            wheels.Remove(wheel);

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = wheel.Segments.FirstOrDefault(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment was not found.");

                wheel.Segments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);
            }

            httpContext.Response.Cookies.Append(PointWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }
    }
}
