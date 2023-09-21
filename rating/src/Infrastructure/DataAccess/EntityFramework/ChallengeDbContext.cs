using Domain.Entities;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Infrastructure.DataAccess.EntityFramework;

public class ChallengeDbContext : DbContext
{
    public DbSet<RatingEntity> Ratings { get; set; }

    public ChallengeDbContext(DbContextOptionsBuilder<ChallengeDbContext> optionsBuilder) : base(optionsBuilder.Options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChallengeDbContext).Assembly);
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);
    }
}