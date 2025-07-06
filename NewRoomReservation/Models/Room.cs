namespace NewRoomReservation.Models;

public class Room
{
    public int Id { get; set; }
    public int Places { get; set; }
    public string Type { get; set; } = "Eco"; 
    public decimal Price { get; set; }
    public bool IsAviable { get; set; } = true;

    
    public List<User>? Users { get; set; }
}