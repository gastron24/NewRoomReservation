using NewRoomReservation.Models;

namespace NewRoomReservation.Entities;

public class Reservation
{
    Room RoomID { get; set; }
    User UserID { get; set; }
    public DateTime StartReservation { get; set; }
    public DateTime EndReservation { get; set; }
}