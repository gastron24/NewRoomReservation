namespace NewRoomReservation.Models;

public class User
{
    public int Id { get; set; }
    public bool IsAdmin { get; set; } = false;
    public bool IsBanned { get; set; } = false;
    public string UserName { get; set; }
    public string Password { get; set; }
    public int RoomHeLives { get; set; } 
    public Room Room { get; set; }
    public decimal Balance { get; set; } 
}
    