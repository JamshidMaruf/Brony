namespace Brony.Models;

public class BookingCreateModel
{
    public int UserId { get; set; }
    public int StadiumId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
