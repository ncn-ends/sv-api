using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Configs;

public class BuildDbContext : DbContext
{
    public DbSet<BuildOrderList> BuildOrderLists { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BuildOrderStep> BuildOrderSteps { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public BuildDbContext(DbContextOptions<BuildDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        AddTimestamps();
        return await base.SaveChangesAsync();
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity) entity.Entity).CreatedAt = now;
            }

            ((BaseEntity) entity.Entity).UpdatedAt = now;
        }
    }
}