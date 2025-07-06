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

        user.RoomId = room.Id;
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

        user.RoomId = room.Id;
        user.Room = room;
        await _db.SaveChangesAsync();

        return Ok("Комната успешно изменена");


    }

    [HttpPost("complaint")]
    public async Task<IActionResult> PushComplaint([FromBody] ComplaintDto dto)
    {
        if(dto == null || string.IsNullOrWhiteSpace(dto.Text))
            return BadRequest("Текст не может быть пустым");
        
        var user = await _db.Users.FindAsync(dto.UserId);
        if (user == null)
            return NotFound("Пользователь не найден");

        var complaint = new Complaint()
        {
            Text = dto.Text,
            UserID = dto.UserId,
        };
        _db.Complaints.Add(complaint);
        await _db.SaveChangesAsync();
        return Ok("Жалоба доставлена");
    }

    [HttpPost("order-service")]
    public async Task<IActionResult> OrderService([FromBody] OrderServiceDto orderDto)
    {
        var user = await _db.Users.FindAsync(orderDto.UserId);
        if (user == null)
            return NotFound("Пользователь не найден");
        
        if (user.Balance < orderDto.Coast)
            return BadRequest("Недостаточно средств");

        user.Balance -= orderDto.Coast;

        var order = new ServiceOrder()
        {
            ServiceName = orderDto.ServiceName,
            Coast =  orderDto.Coast,
            UserId = orderDto.UserId,
        };

        _db.ServiceOrders.Add(order);
        await _db.SaveChangesAsync();
        return Ok("Услуга заказана");
    }
}