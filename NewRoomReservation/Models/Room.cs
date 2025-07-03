namespace NewRoomReservation.Models;

public class Room
{
    public int Id { get; set; }
    public int Places { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }
    public bool IsAviable { get; set; } = true;
    public int? OccupiedUserId { get; set; }
    public User OccupiedByUser { get; set; }
    
    public ICollection<User> Users { get; set; } = new  List<User>();
}