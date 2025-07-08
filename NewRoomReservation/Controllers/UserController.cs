using Microsoft.AspNetCore.Mvc;
using NewRoomReservation.DTO;
using NewRoomReservation.Services;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("reserve-room")]
    public async Task<IActionResult> ReserveRoom(ReserveRoomDto dto)
    {
        bool success = await _userService.ReserveRoomAsync(dto.UserId, dto.RoomId);

        if (!success)
            return BadRequest("Не удалось забронировать комнату");

        return Ok($"Еб*ть мы тебя развели! Твоя халупа {dto.RoomId}");
    }

    [HttpPut("change-room")]
    public async Task<IActionResult> ChangeRoom(ReserveRoomDto dto)
    {
        bool success = await _userService.ChangeRoomAsync(dto.UserId, dto.RoomId);

        if (!success)
            return BadRequest("Не удалось изменить комнату");

        return Ok("Комната успешно изменена");
    }

    [HttpPost("complaint")]
    public async Task<IActionResult> PushComplaint([FromBody] ComplaintDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Text))
            return BadRequest("Текст не может быть пустым");

        await _userService.AddComplaintAsync(dto);
        return Ok("Жалоба доставлена");
    }

    [HttpPost("order-service")]
    public async Task<IActionResult> OrderService([FromBody] OrderServiceDto dto)
    {
        bool success = await _userService.OrderServiceAsync(dto);

        if (!success)
            return BadRequest("Невозможно заказать услугу");

        return Ok("Услуга заказана");
    }
}