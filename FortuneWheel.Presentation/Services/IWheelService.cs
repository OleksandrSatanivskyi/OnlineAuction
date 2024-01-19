using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Services
{
    public interface IWheelService
    {
        Task<IEnumerable<WheelOfFortune>> GetUserWheels(Guid userId);
    }
}
