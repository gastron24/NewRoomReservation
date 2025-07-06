namespace NewRoomReservation.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsBanned { get; set; }
    public decimal Balance { get; set; }
    
    
    public int? RoomId { get; set; }
    public Room? Room { get; set; }
}
    