using NewRoomReservation.DTO;
using NewRoomReservation.Models;

namespace NewRoomReservation.Services;

public interface IUserService
{
    User GetUserByUserId(int UserId);
    Task<bool> ReserveRoomAsync(int userId, int roomId);
    Task<bool> ChangeRoomAsync(int userId, int roomId);
    Task AddComplaintAsync(ComplaintDto dto);
    Task<bool> OrderServiceAsync(OrderServiceDto dto);
}