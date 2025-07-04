using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRoomReservation.Models;
using NewRoomReservation.Infrastructure;

namespace NewRoomReservation.Controllers;

[ApiController]
[Route("/admin[controller]")]
public class AdminController : ControllerBase
{
    private readonly AdminUser _adminUser;
    private readonly Infrastructure.HostelDb _db;
    private readonly User _user;

    public AdminController(AdminUser adminUser, Infrastructure.HostelDb db, User user)
    {
        _adminUser = adminUser;
        _db = db;
        _user = user;
        if (adminUser.IsAdmin == false) 
        { 
            throw new Exception("У вас нету прав администратора."); // проверка на админа => нет прав пошел НАХ. 
        }                                                           // + вопрос (можно ли создавать такие проверки
    }                                                               //            в конструкторе)
    
    [HttpGet]
    public IActionResult GetAllUser()
    {
        if (_adminUser.IsAdmin == false)
            return BadRequest("У вас нет прав администратора.");
        
        var alluser = _db.Users.ToListAsync();
            return Ok(alluser);
    }

    [HttpPut("edit-user/{id}")]
    public async Task<IActionResult> EditUser(int id, [FromBody] User editUser)
    {
        if (_adminUser.IsAdmin == false)
            return BadRequest("У вас нет прав администратора.");
        
        var user = _db.Users.FindAsync(id);
        if (user == null)
            return NotFound("Пользователь не найден");
        
        _user.UserName = editUser.UserName; 
        _user.IsBanned = editUser.IsBanned;
        _user.IsAdmin = editUser.IsAdmin;
        _user.RoomHeLives = editUser.RoomHeLives;
        _user.Balance = editUser.Balance;

        await _db.SaveChangesAsync();
        return Ok("Информация о Пользователе обновлена");
    }

    [HttpPost("ban-user/{id}")]
    public async Task<IActionResult> BanUser(int id, [FromBody] User banedUser)
    {
        if (_adminUser.IsAdmin == false)
            return BadRequest("У вас нет прав Администратора.");
        
        var user = _db.Users.FindAsync(id);
        if (user == null)
            return NotFound("пользователь не найден");
        
        banedUser.IsBanned = true;
        await _db.SaveChangesAsync();
        return Ok("ПОльзователь заблокирован");
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (_adminUser.IsAdmin == false)
            return BadRequest("Нету прав Администратора");

        var user = _db.Users.FindAsync(id);
        if (user == null)
            return NotFound("Пользователь не найден");

        _db.Remove(user);
        await _db.SaveChangesAsync();
        return Ok("Пользователь удален");

    }

    [HttpPost("edit-room/{id}")]
    public async Task<IActionResult> EditRoom(int id, [FromBody] Room updRoom)
    {
        if (_adminUser.IsAdmin == false)
            return BadRequest("Нет прав администратора");
       
        var room = await _db.Rooms.FindAsync(id);
         if (room == null)
             return NotFound("Комната не найдена");
         
         room.Id = updRoom.Id;
         room.Type = updRoom.Type;
         room.Price = updRoom.Price;
         room.IsAviable = updRoom.IsAviable;

         await _db.SaveChangesAsync();
         return Ok("Комната обновлена");
    }
    
    [HttpPost("room/{id}/leaveUser")]
    public async Task<IActionResult> EvictUserFromRoom(int id)
    {
        if (!_adminUser.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms
            .Include(r => r.OccupiedByUser)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
            return NotFound("Комната не найдена");
      
        if (room.OccupiedByUser == null)
            return BadRequest("В комнате никто не проживает");
        
        room.OccupiedUserId = null;
        room.OccupiedByUser = null;
        room.IsAviable = true;

        await _db.SaveChangesAsync();

        return Ok("Пользователь успешно выселен из общаги");
    }
   
    [HttpPost("room/{id}/make-available")]
    public async Task<IActionResult> MakeRoomAvailable(int id)
    {
        if (!_adminUser.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return NotFound("Комната не найдена");

        room.IsAviable = true;
        await _db.SaveChangesAsync();

        return Ok("Комната доступна");
    }
    
    [HttpPost("room/{id}/make-unavailable")]
    public async Task<IActionResult> MakeRoomUnavailable(int id)
    {
        if (!_adminUser.IsAdmin)
            return BadRequest("Нет прав администратора");

        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
            return NotFound("Комната не найдена");

        room.IsAviable = false;
        await _db.SaveChangesAsync();

        return Ok("Комната недоступна");
    }
}