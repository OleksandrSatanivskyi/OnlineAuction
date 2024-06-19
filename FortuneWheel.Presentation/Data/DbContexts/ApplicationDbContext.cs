using OnlineAuc.Domain.Auth;
using OnlineAuc.Domain.Segments;
using OnlineAuc.Domain.Auctions;
using Microsoft.EntityFrameworkCore;

namespace OnlineAuc.Data.DbContexts
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }
        public DbSet<ClassicAuction> ClassicWheels { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<PointAuction> PointWheels { get; set; }
        public DbSet<PointSegment> PointSegments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Segment>()
                .HasOne<ClassicAuction>()
                .WithMany(cw => cw.Segments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Segment>().ToTable("Segments");

            modelBuilder.Entity<PointSegment>()
                .HasOne<PointAuction>()
                .WithMany(pw => pw.Segments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointSegment>().ToTable("PointSegments");
        }
    }

}
