using FortuneWheel.Domain.Auth;
using FortuneWheel.Domain.Segments;
using FortuneWheel.Domain.WheelsOfFortune;
using Microsoft.EntityFrameworkCore;

namespace FortuneWheel.Data.DbContexts
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<WheelOfFortune> WheelsOfFortune { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }

}
