using Microsoft.EntityFrameworkCore;
using NewRoomReservation.DTO;
using NewRoomReservation.Infrastructure;
using NewRoomReservation.Models;

namespace NewRoomReservation.Services;

public class UserService : IUserService
{
    private readonly HostelDb _db;

    public UserService(HostelDb db)
    {
        _db = db;
    }
    
    public User GetUserByUserId(int userId)
    {
        return _db.Users.FirstOrDefault(u => u.Id == userId);

    }
    
    public async Task<bool> ReserveRoomAsync(int userId, int roomId)
    {
        var user = await _db.Users.FindAsync(userId);
        var room = await _db.Rooms.FindAsync(roomId);

        if (user == null || room == null)
            return false;

        if (user.Balance < room.Price)
            return false;

        user.RoomId = room.Id;
        user.Room = room;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeRoomAsync(int userId, int roomId)
    {
        var user = await _db.Users
            .Include(u => u.Room)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var room = await _db.Rooms.FindAsync(roomId);

        if (user == null || room == null)
            return false;

        if (user.Balance < room.Price)
            return false;

        user.RoomId = room.Id;
        user.Room = room;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task AddComplaintAsync(ComplaintDto dto)
    {
        var complaint = new Complaint
        {
            Text = dto.Text,
            UserID = dto.UserId
        };

        await _db.Complaints.AddAsync(complaint);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> OrderServiceAsync(OrderServiceDto dto)
    {
        var user = await _db.Users.FindAsync(dto.UserId);

        if (user == null)
            return false;

        if (user.Balance < dto.Coast)
            return false;

        user.Balance -= dto.Coast;

        var order = new ServiceOrder
        {
            ServiceName = dto.ServiceName,
            Coast = dto.Coast,
            UserId = dto.UserId
        };

        await _db.ServiceOrders.AddAsync(order);
        await _db.SaveChangesAsync();

        return true;
    }
}