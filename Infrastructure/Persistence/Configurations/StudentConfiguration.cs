using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.LastName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(s => s.Email).IsUnique()
            .HasFilter("[IsDeleted] = 0");
        builder.Property(s => s.EnrollmentDate).IsRequired();

        builder.Property(s => s.Status)
            .HasConversion(v => (int)v,
                           v => (EnrollmentStatus)v);
    }
}