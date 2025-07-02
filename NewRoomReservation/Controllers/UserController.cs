using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewRoomReservation.DTO;
using NewRoomReservation.Infrastructure;
using NewRoomReservation.Models;

namespace NewRoomReservation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly HostelDb _db;

    public UserController(HostelDb db)
    {
        _db = db;
    }

    [HttpPost("reserve-room")]
    public async Task<IActionResult> ReserveRoom(ReserveRoomDto dto)
    {
        var user = await _db.Users.FindAsync(dto.UserId);
        var room = await _db.Rooms.FindAsync(dto.RoomId);

        if (user == null || room == null)
            return NotFound("Уе**к или Комната не найденны");

        if (user.Balance < room.Price)
            return BadRequest("Бомж пошел на**й отсюда");

        user.RoomHeLives = room.Id;
        user.Room = room;

        await _db.SaveChangesAsync();
        return Ok($"Еб*ть мы тебя развели! Твоя халупа {room.Id} ");

    }

    [HttpPut("change-room")]
    public async Task<IActionResult> ChangeRoom(ReserveRoomDto dto)
    {
        var user = await _db.Users
            .Include(u => u.Room)
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);

        var room = await _db.Rooms.FindAsync(dto.RoomId);

        if (user == null || room == null)
            return NotFound("Кто ты? воин(не найдено)");

        if (user.Balance < room.Price)
            return BadRequest("Деньга нет, положи потом общатсья будем");

        user.RoomHeLives = room.Id;
        user.Room = room;
        await _db.SaveChangesAsync();

        return Ok("Комната успешно изменена");


    }

    [HttpPost("complaint")]
    public async Task<IActionResult> PushComplaint([FromBody] string text, int userId)
    {
        var user = await _db.Users.FindAsync(userId);

        if (user == null)
            return NotFound("Пользователь не найден");

        var complaint = new Complaint()
        {
            Text = text,
            UserID = userId
        };
        _db.Complaints.Add(complaint);
        await _db.SaveChangesAsync();
        return Ok("Жалоба доставлена");
    }

    [HttpPost("order-service")]
    public async Task<IActionResult> OrderService(string serviceName, decimal coast, int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
            return NotFound("Пользователь не найден");
        
        if (user.Balance < coast)
            return BadRequest("Недостаточно средств");

        user.Balance -= coast;

        var order = new ServiceOrder()
        {
            ServiceName = serviceName,
            Coast = coast,
            UserId = userId
        };

        _db.ServiceOrders.Add(order);
        await _db.SaveChangesAsync();
        return Ok("Услуга заказана");
    }
}