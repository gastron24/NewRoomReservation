using NewRoomReservation.Models;

namespace NewRoomReservation.Services;

public interface IAdminService
{
    Task<List<User>> GetAllUsersAsync(int adminId);
    
    Task<bool> EditUserAsync(int id, int adminId, User editUser);
    Task<bool> BanUserAsync(int id, int adminId);
    Task<bool> DeleteUserAsync(int id, int adminId);

    Task<bool> EditRoomAsync(int id, int adminId, Room room);
    
    Task<bool> EvictUserFromRoomAsync(int id, int adminId);
    Task<bool> MakeRoomAvailableAsync(int id, int adminId);
    Task<bool> MakeRoomUnavailableAsync(int id, int adminId);
}