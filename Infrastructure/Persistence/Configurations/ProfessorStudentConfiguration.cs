using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProfessorStudentConfiguration : IEntityTypeConfiguration<ProfessorStudent>
{
    public void Configure(EntityTypeBuilder<ProfessorStudent> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.HasIndex(ps => new {ps.StudentId, ps.ProfessorId})
            .IsUnique();

        builder.HasOne(ps => ps.Student)
            .WithMany(s => s.ProfessorStudents)
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ps => ps.Professor)
            .WithMany(p => p.ProfessorStudents)
            .HasForeignKey(ps => ps.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}