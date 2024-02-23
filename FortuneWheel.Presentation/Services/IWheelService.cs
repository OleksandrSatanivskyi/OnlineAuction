using FortuneWheel.Domain.WheelsOfFortune;
using FortuneWheel.Models.Wheels;

namespace FortuneWheel.Services
{
    public interface IWheelService
    {
        Task CreateWheel(CreateWheelModel model);
        Task<List<WheelItem>> GetAll(Guid userId);
        Task Remove(Guid id, WheelType type);
        Task<ClassicWheel> GetClassicWheel(Guid id);
        Task<PointWheel> GetPointWheel(Guid id);
        Task AddPointSegment(Guid wheelId, string title, uint points, string colorHex = "");
        Task DeletePointWheelSegment(Guid segmentId);
        Task UpdatePointOptions(UpdatePointOptionsModel model);
    }
}
