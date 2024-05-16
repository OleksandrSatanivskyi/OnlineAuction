using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using WheelOfFortune.Domain.Segments;
using WheelOfFortune.Domain.WheelsOfFortune;
using WheelOfFortune.Exceptions;
using WheelOfFortune.Models.Wheels;

namespace FortuneWheel.Services
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
                throw new NotFoundException($"Wheel with Id {wheelId} not found.");

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

        public async Task AddPointSegment(Guid wheelId, string title, uint points, string colorHex, HttpContext httpContext)
        {
            var wheels = await GetPointWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {wheelId} not found.");

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

        public async Task CreateWheel(CreateWheelModel model, HttpContext httpContext)
        {
            switch (model.Type)
            {
                case WheelType.Classic:
                    var classicWheels = await GetClassicWheels(httpContext);
                    var classicWheel = new ClassicWheel(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);

                    classicWheels.Add(classicWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(classicWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays) 
                    });
                    break;

                case WheelType.Point:
                    var pointWheels = await GetPointWheels(httpContext);
                    var pointWheel = new PointWheel(Guid.NewGuid(), model.Title, DateTime.UtcNow, (Guid)model.UserId);

                    pointWheels.Add(pointWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(pointWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
                    });
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }

        }

        public async Task DeleteClassicWheelSegment(Guid segmentId, HttpContext httpContext)
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

        public async Task DeletePointWheelSegment(Guid segmentId, HttpContext httpContext)
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

        public async Task<List<WheelItem>> GetAll(HttpContext httpContext)
        {
            var classicWheels = await GetClassicWheels(httpContext);
            var classicWheelsItems = classicWheels
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, WheelType.Classic))
                .ToList();

            var pointWheels = await GetPointWheels(httpContext);
            var pointWheelsItems = pointWheels
                .Select(c => new WheelItem(c.Id, c.Title, c.CreationDate, c.UserId, c.Segments.Count, WheelType.Point))
                .ToArray();

            var allWheels = classicWheelsItems
                .Concat(pointWheelsItems)
                .OrderByDescending(w => w.CreationDate)
                .ToList();

            return allWheels;
        }

        public async Task<List<ClassicWheel>> GetClassicWheels(HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[ClassicWheels];
            var wheels = JsonConvert.DeserializeObject<List<ClassicWheel>>(wheelsData);

            return wheels;
        }

        public async Task<List<PointWheel>> GetPointWheels(HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[PointWheels];
            var wheels = JsonConvert.DeserializeObject<List<PointWheel>>(wheelsData);

            return wheels;
        }

        public async Task<ClassicWheel> GetClassicWheel(Guid wheelId, HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[ClassicWheels];
            var wheels = JsonConvert.DeserializeObject<List<ClassicWheel>>(wheelsData);

            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {wheelId} not found.");

            return wheel;
        }

        public async Task<PointWheel> GetPointWheel(Guid wheelId, HttpContext httpContext)
        {
            var wheelsData = httpContext.Request.Cookies[PointWheels];
            var wheels = JsonConvert.DeserializeObject<List<PointWheel>>(wheelsData);

            var wheel = wheels.FirstOrDefault(w => w.Id == wheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {wheelId} not found.");

            return wheel;
        }

        public async Task Remove(Guid wheelId, WheelType type, HttpContext httpContext)
        {
            switch (type)
            {
                case WheelType.Classic:
                    var classicWheels = await GetClassicWheels(httpContext);
                    var classicWheel = classicWheels.FirstOrDefault(c => c.Id == wheelId);

                    if (classicWheel == null)
                        throw new NotFoundException($"Wheel with Id {classicWheel.Id} not found.");

                    classicWheels.Remove(classicWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(classicWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
                    });
                    break;

                case WheelType.Point:
                    var pointWheels = await GetPointWheels(httpContext);
                    var pointWheel = pointWheels.FirstOrDefault(c => c.Id == wheelId);

                    if (pointWheel == null)
                        throw new NotFoundException($"Wheel with Id {pointWheel.Id} not found.");

                    pointWheels.Remove(pointWheel);

                    httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(pointWheels), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
                    });
                    break;

                default:
                    throw new InvalidOperationException("Unsupported type of wheel");
            }
        }

        public async Task UpdateClassicWheelOptions(UpdateClassicWheelOptionsModel model, HttpContext httpContext)
        {
            var wheels = await GetClassicWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == model.WheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {model.WheelId} not found.");

            wheels.Remove(wheel);

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = wheel.Segments.FirstOrDefault(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment with id {oldSegment} not found.");

                wheel.Segments.Remove(oldSegment);
                wheel.Segments.Add(newSegment);
            }

            httpContext.Response.Cookies.Append(ClassicWheels, JsonConvert.SerializeObject(wheels), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(CookieExpirationDays)
            });
        }

        public async Task UpdatePointWheelOptions(UpdatePointWheelOptionsModel model, HttpContext httpContext)
        {
            var wheels = await GetPointWheels(httpContext);
            var wheel = wheels.FirstOrDefault(w => w.Id == model.WheelId);

            if (wheel == null)
                throw new NotFoundException($"Wheel with Id {model.WheelId} not found.");

            wheels.Remove(wheel);

            foreach (var newSegment in model.Segments)
            {
                var oldSegment = wheel.Segments.FirstOrDefault(p => p.Id == newSegment.Id);

                if (oldSegment == null)
                    throw new NotFoundException($"Segment with id {oldSegment} not found.");

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
