using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityFramework.Configuration;

public sealed class RatingEntityTypeConfiguration : IEntityTypeConfiguration<RatingEntity>
{
    private const string IntType = "oid";
    public void Configure(EntityTypeBuilder<RatingEntity> builder)
    {
        builder.ToTable("ratings");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new
        {
            x.Id,
            x.CraftsmanId
        });
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(x => x.Score)
            .HasColumnType(IntType)
            .HasColumnName("score");

        builder.Property(x => x.CraftsmanId)
            .HasColumnType(IntType)
            .HasColumnName("craftsman_id");

        builder.Property(x => x.CustomerId)
            .HasColumnType(IntType)
            .HasColumnName("customer_id");

        builder.Property(x => x.TaskId)
            .HasColumnType(IntType)
            .HasColumnName("task_id");

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .ValueGeneratedOnAdd();
    }
}