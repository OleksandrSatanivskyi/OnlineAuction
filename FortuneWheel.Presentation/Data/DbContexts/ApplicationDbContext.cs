using FortuneWheel.Domain.Auth;
using FortuneWheel.Domain.Segments;
using FortuneWheel.Domain.WheelsOfFortune;
using Microsoft.EntityFrameworkCore;

namespace FortuneWheel.Data.DbContexts
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }
        public DbSet<ClassicWheel> ClassicWheels { get; set; }
        public DbSet<PointWheel> PointWheels { get; set; }
        public DbSet<PointSegment> PointSegments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }

}
