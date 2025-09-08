using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Professor> Professors { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ProfessorStudent> ProfessorStudents { get; set; }
    public DbSet<SeedHistory> SeedHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Student>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Professor>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<ProfessorStudent>().HasQueryFilter(ps => !ps.Student.IsDeleted && !ps.Professor.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
    public override int SaveChanges()
    {
        //foreach (var entry in ChangeTracker.Entries())
        //{
        //    if (entry.State == EntityState.Deleted)
        //    {
        //        if (entry.Entity is Student student)
        //        {
        //            entry.State = EntityState.Modified;
        //            student.IsDeleted = true;
        //        }
        //        else if (entry.Entity is Professor professor)
        //        {
        //            entry.State = EntityState.Modified;
        //            professor.IsDeleted = true;
        //        }
        //    }
        //}

        SoftDelete();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //foreach (var entry in ChangeTracker.Entries())
        //{
        //    if (entry.State == EntityState.Deleted)
        //    {
        //        if (entry.Entity is Student student)
        //        {
        //            entry.State = EntityState.Modified;
        //            student.IsDeleted = true;
        //        }
        //        else if (entry.Entity is Professor professor)
        //        {
        //            entry.State = EntityState.Modified;
        //            professor.IsDeleted = true;
        //        }
        //    }
        //}

        SoftDelete();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void SoftDelete()
    {
        foreach(var entry in ChangeTracker.Entries<SoftDeletableEntity>())
        {
            if(entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }
        }
    }
}