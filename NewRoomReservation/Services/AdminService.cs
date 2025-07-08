using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Infrastructure;
using NewRoomReservation.Models;

namespace NewRoomReservation.Services;

    public class AdminService : IAdminService
{
    private readonly HostelDb _db;

    public AdminService(HostelDb db)
    {
        _db = db;
    }

    public async Task<List<User>> GetAllUsersAsync(int adminId)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            throw new Exception("Нету Прав Администратора");

        return await _db.Users.ToListAsync();
    }

    public async Task<bool> EditUserAsync(int id, int adminId, User editUser)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var targetUser = await _db.Users.FindAsync(id);
        if (targetUser == null)
            return false;

        targetUser.UserName = editUser.UserName;
        targetUser.IsBanned = editUser.IsBanned;
        targetUser.IsAdmin = editUser.IsAdmin;
        targetUser.RoomId = editUser.RoomId;
        targetUser.Balance = editUser.Balance;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> BanUserAsync(int id, int adminId)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return false;

        user.IsBanned = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id, int adminId)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditRoomAsync(int id, int adminId, Room room)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var targetRoom = await _db.Rooms.FindAsync(id);
        if (targetRoom == null)
            return false;

        targetRoom.Type = room.Type;
        targetRoom.Price = room.Price;
        targetRoom.IsAviable = room.IsAviable;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EvictUserFromRoomAsync(int id, int adminId)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var room = await _db.Rooms
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null || room.Users == null)
            return false;

        room.Users = null;
        room.IsAviable = true;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MakeRoomAvailableAsync(int id, int adminId)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return false;

        room.IsAviable = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MakeRoomUnavailableAsync(int id, int adminId)
    {
        var admin = await _db.Users.FindAsync(adminId);
        if (admin == null || !admin.IsAdmin)
            return false;

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return false;

        room.IsAviable = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
