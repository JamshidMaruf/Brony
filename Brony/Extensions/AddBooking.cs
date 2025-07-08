using Brony.Domain;
namespace Brony.Extensions;

public static class AddBooking
{
    #region AddBooking()
    public static void AddingBooking(this List<Booking> bookings, int bookingId, int userId, int stadiumId, DateTime startTime, DateTime endTime, decimal price)
    {
        var booking = new Booking()
        {
            Id = bookingId,
            UserId = userId,
            StadiumId = stadiumId,
            StartTime = startTime,
            EndTime = endTime,
            Price = price
        };

        bookings.Add(booking);
    }
    #endregion
}
