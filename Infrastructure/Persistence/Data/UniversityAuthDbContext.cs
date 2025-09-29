using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public class UniversityAuthDbContext : IdentityDbContext
{
    public UniversityAuthDbContext(DbContextOptions<UniversityAuthDbContext> options) : base(options){ }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var readerRoleId = "f360aaa8-ef5c-48f5-9712-70e197d2573a";

        var writerRoleId = "2cfae2bc-1284-4eee-90c0-f274d5a48485";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        base.OnModelCreating(builder);
    }
}