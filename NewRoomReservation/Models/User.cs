namespace NewRoomReservation.Models;

public class User
{
    public int Id { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string UserName { get; set; }
    public string Password { get; set; }
    public int RoomHeLives { get; set; } = 0;
    public Room Room { get; set; }
    public decimal Balance { get; set; } 

    public User()
    {
        Id = Id++;
    }

   
    public User user1 = new User      // Тестовые Юзеры. 
    {
        IsAdmin = false,
        UserName = "Vasily",
        Password = "VasiliyTerminator",
        RoomHeLives = 0,
        Balance = 0,
    };

    public User user2 = new User()
    {
        IsAdmin = false,
        UserName = "Anton",
        Password = "AntonSith",
        RoomHeLives = 1,
        Balance = 1500,
    };

    public User user3 = new User()
    {
        IsAdmin = true,
        UserName = "AdmUser",
        Password = "SuperPassRonaldoGoal",
        RoomHeLives = 999,
        Balance = 999999
    };

}
    