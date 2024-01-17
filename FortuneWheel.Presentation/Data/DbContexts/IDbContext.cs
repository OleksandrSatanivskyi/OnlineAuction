using FortuneWheel.Domain;
using Microsoft.EntityFrameworkCore;

namespace FortuneWheel.Data.DbContexts
{
    public interface IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<WheelOfFortune> WheelsOfFortune { get; set; }
        DbSet<WheelSegment> Segments { get; set; }
        DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
    }
}
