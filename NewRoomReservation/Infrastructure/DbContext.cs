using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Models;

namespace NewRoomReservation.Infrastructure;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options)
        : base(options) { }

    private DbSet<User> Users { get; set; }
    private DbSet<Room> Rooms { get; set; }
    







}