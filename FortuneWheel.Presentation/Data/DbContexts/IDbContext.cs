using FortuneWheel.Domain.Auth;
using FortuneWheel.Domain.Segments;
using FortuneWheel.Domain.WheelsOfFortune;
using Microsoft.EntityFrameworkCore;

namespace FortuneWheel.Data.DbContexts
{
    public interface IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<ClassicWheel> ClassicWheels { get; set; }
        DbSet<PointWheel> PointWheels { get; set; }
        DbSet<Segment> Segments { get; set; }
        DbSet<PointSegment> PointSegments { get; set; }
        DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
    }
}
