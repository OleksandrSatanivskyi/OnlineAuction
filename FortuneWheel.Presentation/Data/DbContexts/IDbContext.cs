using OnlineAuc.Domain.Auth;
using OnlineAuc.Domain.Segments;
using OnlineAuc.Domain.Auctions;
using Microsoft.EntityFrameworkCore;

namespace OnlineAuc.Data.DbContexts
{
    public interface IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<ClassicAuction> ClassicWheels { get; set; }
        DbSet<PointAuction> PointWheels { get; set; }
        DbSet<Segment> Segments { get; set; }
        DbSet<PointSegment> PointSegments { get; set; }
        DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
    }
}
