using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Models;

namespace NewRoomReservation.Infrastructure;

public class HostelDb : DbContext
{
    public HostelDb(DbContextOptions<HostelDb> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasOne(r => r.OccupiedByUser)
            .WithMany()
            .HasForeignKey(k => k.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}