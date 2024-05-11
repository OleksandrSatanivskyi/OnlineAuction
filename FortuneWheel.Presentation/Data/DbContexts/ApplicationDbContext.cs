﻿using WheelOfFortune.Domain.Auth;
using WheelOfFortune.Domain.Segments;
using WheelOfFortune.Domain.WheelsOfFortune;
using Microsoft.EntityFrameworkCore;

namespace WheelOfFortune.Data.DbContexts
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UnconfirmedEmail> UnconfirmedEmails { get; set; }
        public DbSet<ClassicWheel> ClassicWheels { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<PointWheel> PointWheels { get; set; }
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
                .HasOne<ClassicWheel>()
                .WithMany(cw => cw.Segments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Segment>().ToTable("Segments");

            modelBuilder.Entity<PointSegment>()
                .HasOne<PointWheel>()
                .WithMany(pw => pw.Segments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointSegment>().ToTable("PointSegments");
        }
    }

}
