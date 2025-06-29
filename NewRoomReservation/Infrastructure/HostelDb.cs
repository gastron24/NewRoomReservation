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
    







}