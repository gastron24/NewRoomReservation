using Microsoft.AspNetCore.Mvc;
using NewRoomReservation.Models;
using NewRoomReservation.Services;

[ApiController]
[Route("api/admin/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    [HttpGet("GetAllUser")]
    public async Task<IActionResult> GetAllUser(int userId)
    {
        var users = await _adminService.GetAllUsersAsync(userId);
        if (users == null || !users.Any())
            return BadRequest("Нет доступа или пользователи не найдены");

        return Ok(users);
    }
    [HttpPut("edit-user/{id}")]
    public async Task<IActionResult> EditUser(int id, int userId, [FromBody] User editUser)
    {
        bool success = await _adminService.EditUserAsync(id, userId, editUser);
        if (!success)
            return BadRequest("Не удалось обновить пользователя");

        return Ok("Информация о пользователе обновлена");
    }
    [HttpPost("ban-user/{id}")]
    public async Task<IActionResult> BanUser(int id, int userId)
    {
        bool success = await _adminService.BanUserAsync(id, userId);
        if (!success)
            return BadRequest("Не удалось заблокировать пользователя");

        return Ok("Пользователь заблокирован");
    }
    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id, int userId)
    {
        bool success = await _adminService.DeleteUserAsync(id, userId);
        if (!success)
            return BadRequest("Не удалось удалить пользователя");

        return Ok("Пользователь удален");
    }
    [HttpPost("edit-room/{id}")]
    public async Task<IActionResult> EditRoom(int id, int userId, [FromBody] Room updRoom)
    {
        bool success = await _adminService.EditRoomAsync(id, userId, updRoom);
        if (!success)
            return BadRequest("Не удалось обновить комнату");

        return Ok("Комната обновлена");
    }
    [HttpPost("room/{id}/leaveUser")]
    public async Task<IActionResult> EvictUserFromRoom(int id, int userId)
    {
        bool success = await _adminService.EvictUserFromRoomAsync(id, userId);
        if (!success)
            return BadRequest("Не удалось выселить пользователя");

        return Ok("Пользователь успешно выселен из общаги");
    }
    [HttpPost("room/{id}/make-available")]
    public async Task<IActionResult> MakeRoomAvailable(int id, int userId)
    {
        bool success = await _adminService.MakeRoomAvailableAsync(id, userId);
        if (!success)
            return BadRequest("Не удалось сделать комнату доступной");

        return Ok("Комната доступна");
    }
    [HttpPost("room/{id}/make-unavailable")]
    public async Task<IActionResult> MakeRoomUnavailable(int id, int userId)
    {
        bool success = await _adminService.MakeRoomUnavailableAsync(id, userId);
        if (!success)
            return BadRequest("Не удалось сделать комнату недоступной");

        return Ok("Комната недоступна");
    }
}