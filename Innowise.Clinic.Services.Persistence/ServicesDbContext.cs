using Innowise.Clinic.Services.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Services.Persistence;

public class ServicesDbContext : DbContext
{
    public ServicesDbContext(DbContextOptions<ServicesDbContext> options) : base(options)
    {
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Service>().HasKey(x => x.ServiceId);
        modelBuilder.Entity<Service>().Property(x => x.Name)
            .HasColumnType("nvarchar(256)");
        modelBuilder.Entity<Service>().Property(x => x.Price).HasColumnType("decimal(10,6)");

        modelBuilder.Entity<Service>().HasOne(x => x.Specialization)
            .WithMany(sp => sp.Services)
            .HasForeignKey(x => x.SpecializationId);

        modelBuilder.Entity<Specialization>().HasKey(x => x.SpecializationId);
        modelBuilder.Entity<Specialization>().Property(x => x.Name)
            .HasColumnType("nvarchar(256)");
    }
}