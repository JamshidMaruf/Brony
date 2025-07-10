namespace Brony.Constants;

public class PathHolder
{
<<<<<<< HEAD
    private static readonly string parentRoot = 
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    public static readonly string UsersFilePath = Path.Combine(parentRoot,"Data","Users.txt");
    public static readonly string BookingsFilePath = Path.Combine(parentRoot,"Data","Bookings.txt");
=======
    public const string UsersFilePath = "Data/Users.json";
    public const string BookingsFilePath = "Data/Users.json";
    public const string UserIdPath = "Data/UsersId.json";
    public const string StadiumIdPath = "Data/StadiumId.json";
    public const string BookingIdPath = "Data/BookingId.json";
>>>>>>> c3d3f596d441d2dc79d9c76eb851231ffe05ba75
}
