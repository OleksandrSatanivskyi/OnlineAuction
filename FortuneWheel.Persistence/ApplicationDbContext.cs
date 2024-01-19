using FortuneWheel.Domain;
using Microsoft.EntityFrameworkCore;

namespace FortuneWheel.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<WheelOfFortune> WheelsOfFortune { get; set; }
        public DbSet<WheelSegment> Segments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }

}
