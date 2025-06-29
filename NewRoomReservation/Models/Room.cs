namespace NewRoomReservation.Models;

public class Room
{
    public int Id { get; set; }
    public int Places { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}