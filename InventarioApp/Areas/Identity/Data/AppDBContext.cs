using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventarioApp.Models;

namespace InventarioApp.Areas.Identity.Data;

public class AppDBContext : IdentityDbContext<AppUser>
{
    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");
        builder.Entity<AppUser>(
            entity => 
            {
                entity.ToTable(name: "User");
            });

        builder.Entity<IdentityRole>(
            entity =>
            {
                entity.ToTable(name: "Role");
            });

        builder.Entity<IdentityUserRole<string>>(
            entity =>
            {
                entity.ToTable("UserRole");
            });

        builder.Entity<IdentityUserClaim<string>>(
            entity =>
            {
                entity.ToTable("UserClaim");
            });

        builder.Entity<IdentityUserLogin<string>>(
            entity =>
            {
                entity.ToTable("UserLogin");
            });

        builder.Entity<IdentityRoleClaim<string>>(
            entity =>
            {
                entity.ToTable("RoleClaim");
            });

        builder.Entity<IdentityUserToken<string>>(
            entity =>
            {
                entity.ToTable("UserToken");
            });



        builder.ApplyConfiguration(new AppUserEntityConfiguration());
    }

    public DbSet<InventoryEntry> InventoryEntry { get; set; } = default!;
}
public class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(p => p.FirstName).HasMaxLength(50);
        builder.Property(p => p.LastName).HasMaxLength(50);
    }
}