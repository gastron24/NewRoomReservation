using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Models;
using NewRoomReservation.Infrastructure;

namespace NewRoomReservation.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
public class AdminController : ControllerBase
{
    private readonly HostelDb _db;

    public AdminController(HostelDb db)
    {
        _db = db;
    }
    
    [HttpGet("GetAllUser")]
    public async Task<IActionResult> GetAllUser(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null || !user.IsAdmin)
            return BadRequest("У вас нет прав администратора.");

        var users = await _db.Users.ToListAsync();
        return Ok(users);
    }
    [HttpPut("edit-user/{id}")]
    public async Task<IActionResult> EditUser(int id, int userId, [FromBody] User editUser)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("У вас нет прав администратора.");

        var targetUser = await _db.Users.FindAsync(id);
        if (targetUser == null)
            return NotFound("Пользователь не найден");

        targetUser.UserName = editUser.UserName;
        targetUser.IsBanned = editUser.IsBanned;
        targetUser.IsAdmin = editUser.IsAdmin;
        targetUser.RoomId = editUser.RoomId;
        targetUser.Balance = editUser.Balance;

        await _db.SaveChangesAsync();
        return Ok("Информация о пользователе обновлена");
    }
    [HttpPost("ban-user/{id}")]
    public async Task<IActionResult> BanUser(int id, int userId)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("У вас нет прав администратора.");

        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound("Пользователь не найден");

        user.IsBanned = true;
        await _db.SaveChangesAsync();

        return Ok("Пользователь заблокирован");
    }
    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id, int userId)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("Нету прав администратора");

        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound("Пользователь не найден");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return Ok("Пользователь удален");
    }
    [HttpPost("edit-room/{id}")]
    public async Task<IActionResult> EditRoom(int id, int userId, [FromBody] Room updRoom)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return NotFound("Комната не найдена");

        room.Type = updRoom.Type;
        room.Price = updRoom.Price;
        room.IsAviable = updRoom.IsAviable;

        await _db.SaveChangesAsync();
        return Ok("Комната обновлена");
    }
    [HttpPost("room/{id}/leaveUser")]
    public async Task<IActionResult> EvictUserFromRoom(int id, int userId)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
            return NotFound("Комната не найдена");

        if (room.Users == null)
            return BadRequest("В комнате никто не проживает");

        room.Users = null;
        room.IsAviable = true;

        await _db.SaveChangesAsync();

        return Ok("Пользователь успешно выселен из общаги");
    }
    [HttpPost("room/{id}/make-available")]
    public async Task<IActionResult> MakeRoomAvailable(int id, int userId)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return NotFound("Комната не найдена");

        room.IsAviable = true;
        await _db.SaveChangesAsync();

        return Ok("Комната доступна");
    }
    [HttpPost("room/{id}/make-unavailable")]
    public async Task<IActionResult> MakeRoomUnavailable(int id, int userId)
    {
        var admin = await _db.Users.FindAsync(userId);
        if (admin == null || !admin.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return NotFound("Комната не найдена");

        room.IsAviable = false;
        await _db.SaveChangesAsync();

        return Ok("Комната недоступна");
    }
}