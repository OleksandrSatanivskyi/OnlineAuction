using FortuneWheel.Domain.WheelsOfFortune;
using FortuneWheel.Models.Wheels;

namespace FortuneWheel.Services
{
    public interface IWheelService
    {
        Task<List<WheelItem>> GetUserWheels(Guid userId);
    }
}
