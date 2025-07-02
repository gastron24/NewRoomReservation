namespace NewRoomReservation.Models;

public class Complaint
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int UserID { get; set; }
    public User User { get; set; }
}