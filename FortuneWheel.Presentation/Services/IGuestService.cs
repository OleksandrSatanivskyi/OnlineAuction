using WheelOfFortune.Domain.WheelsOfFortune;
using WheelOfFortune.Models.Wheels;

namespace FortuneWheel.Services
{
    public interface IGuestService
    {
        Task AddClassicSegment(Guid id, string title, string colorHex, HttpContext httpContext);
        Task AddPointSegment(Guid id, string title, uint points, string colorHex, HttpContext httpContext);
        Task CreateWheel(CreateWheelModel model, HttpContext httpContext);
        Task DeleteClassicWheelSegment(Guid id, HttpContext httpContext);
        Task DeletePointWheelSegment(Guid id, HttpContext httpContext);
        Task<List<WheelItem>> GetAll(HttpContext httpContext);
        Task<ClassicWheel> GetClassicWheel(Guid id, HttpContext httpContext);
        Task<PointWheel> GetPointWheel(Guid id, HttpContext httpContext);
        Task Remove(Guid id, WheelType type, HttpContext httpContext);
        Task UpdateClassicWheelOptions(UpdateClassicWheelOptionsModel model, HttpContext httpContext);
        Task UpdatePointWheelOptions(UpdatePointWheelOptionsModel model, HttpContext httpContext);
    }
}
