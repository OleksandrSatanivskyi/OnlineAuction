using FortuneWheel.Data.DbContexts;
using FortuneWheel.Domain.WheelsOfFortune;

namespace FortuneWheel.Services
{
    public class WheelService : IWheelService
    {
        private readonly IDbContext DbContext;

        public WheelService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task<IEnumerable<WheelOfFortune>> GetUserWheels(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
