namespace NewRoomReservation.Models;

public class AdminUser
{
    public int Id { get; set; } 
    public string AdminName { get; set; }
    public string AdminPassword { get; set; }
    public bool IsAdmin { get; set; } = true;

}