namespace NewRoomReservation.Models;

public class ServiceOrder
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public decimal Coast { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}