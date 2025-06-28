namespace NewRoomReservation.Models;

public class User
{
    public int UserId { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string UserName { get; set; }
    public string Password { get; set; }
    public int RoomHeLives { get; set; } 
    public decimal Balance { get; set; }
    
    public User()
    {
        UserId = UserId++;
    }
    
}