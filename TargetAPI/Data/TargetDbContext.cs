using Microsoft.EntityFrameworkCore;

namespace TargetAPI.Data;

public class TargetDbContext : DbContext
{
    public TargetDbContext(DbContextOptions<TargetDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AS_SC_UTF8");

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasKey(p => p.Login);
        modelBuilder.Entity<User>().Property(p => p.Login).HasMaxLength(15);
        
        modelBuilder.Entity<User>().Property(p => p.Name).HasMaxLength(50);
        modelBuilder.Entity<User>().Property(p => p.Surname).HasMaxLength(80);
        modelBuilder.Entity<User>().Property(p => p.Hash).HasMaxLength(45);
        
        base.OnModelCreating(modelBuilder);
    }
}