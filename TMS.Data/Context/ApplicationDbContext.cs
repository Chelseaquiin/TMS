
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMS.Data.Configurations;
using TMS.Models.Entities;

namespace TMS.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>()
           .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();
            //.IsClustered(false);



            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(e => e.UserId);

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(e => e.UserId);


            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

        }


        public DbSet<Tasks> Tasks { get; set; }
    }
}
