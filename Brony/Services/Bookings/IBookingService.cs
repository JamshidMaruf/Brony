using Brony.Domain;
using Brony.Models;

namespace Brony.Services.Bookings;

public interface IBookingService
{
    void Book(BookingCreateModel createModel);

    void Cancel(int bookingId);

    void ChangeDateTime(int bookingId, DateTime startDate, DateTime endDate);
    
    BookingViewModel Get(int id);

    List<Booking> GetAll();

    List<Booking> GetAllByUserId(int userId);
    
    List<Booking> GetAllByStadiumId(int stadiumId);
}