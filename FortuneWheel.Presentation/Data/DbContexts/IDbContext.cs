using WheelOfFortune.Domain.Auth;
using WheelOfFortune.Domain.Segments;
using WheelOfFortune.Domain.WheelsOfFortune;
using Microsoft.EntityFrameworkCore;

namespace WheelOfFortune.Data.DbContexts
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
