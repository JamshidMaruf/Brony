using System.Reflection.Emit;
using Brony.Constants;
using Brony.Domain;
using Brony.Extensions;
using Brony.Helpers;
using Brony.Services.Stadiums;
using Brony.Services.Users;

namespace Brony.Services.Bookings;

public class BookingService : IBookingService
{
    private int bookingId;
    private readonly List<Booking> bookings;
    private readonly ObjectHolder objectHolder;
    public BookingService(ObjectHolder objectHolder)
    {
        bookingId = 1;
        bookings = new List<Booking>();
        this.objectHolder = objectHolder;
    }

    public void Book(BookingCreateModel createModel)
    {
        var text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        var convertedUsers = text.ToObject<List<Booking>>();


        // check user
        objectHolder.UserService.Get();
        
        // check stadium
        var existStadium = objectHolder.StadiumService.Get(stadiumId);
        
        var startTimePeaces = existStadium.StartWorkingTime.Split(':');
        var startTimeHour = Convert.ToInt32(startTimePeaces[0]);
        
        var endTimePeaces = existStadium.EndWorkingTime.Split(':');
        var endTimeHour = Convert.ToInt32(endTimePeaces[0]);

        if (startTimeHour >= startTime.Hour || endTimeHour <= endTime.Hour)
        {
            throw new Exception($"Stadium does not work in this time! | " +
                                $"Working time: {existStadium.StartWorkingTime} - {existStadium.EndWorkingTime}");
        }
        
        // check time which is not booked
        foreach (var item in bookings)
        {
            if (item.StadiumId == stadiumId)
            {
                if (item.StartTime >= startTime || item.EndTime <= endTime)
                {
                    throw new Exception("This stadium is already booked in this time");
                }
            }
        }
        
        var booking = new Booking
        {
            // Each ID is saved to separate path
            Id = IdGeneration.IdGenerate(Constants.PathHolder.BookingIdPath),
            UserId = IdGeneration.IdGenerate(Constants.PathHolder.UserIdPath),
            StadiumId = IdGeneration.IdGenerate(Constants.PathHolder.StadiumIdPath),
            StartTime = startTime,
            EndTime = endTime,
            Price = existStadium.Price
        };
        
        bookings.Add(booking);
    }

    public void Cancel(int bookingId)
    {
        var existBooking = bookings.Find(x => x.Id == bookingId);

        if (existBooking == null)
        {
            throw new Exception("Booking is not found");
        }

        bookings.Remove(existBooking);
    }

    public void ChangeDateTime(int bookingId, DateTime startTime, DateTime endTime)
    {
        var existBooking = bookings.Find(x => x.Id == bookingId);

        if (existBooking == null)
        {
            throw new Exception("Booking is not found");
        }

        var stadium = objectHolder.StadiumService.Get(existBooking.StadiumId);
        
        var startTimePeaces = stadium.StartWorkingTime.Split(':');
        var startTimeHour = Convert.ToInt32(startTimePeaces[0]);
        
        var endTimePeaces = stadium.EndWorkingTime.Split(':');
        var endTimeHour = Convert.ToInt32(endTimePeaces[0]);

        if (startTimeHour >= startTime.Hour || endTimeHour <= endTime.Hour)
        {
            throw new Exception($"Stadium does not work in this time! | " +
                                $"Working time: {stadium.StartWorkingTime} - {stadium.EndWorkingTime}");
        }
        
        foreach (var item in bookings)
        {
            if (item.StadiumId == existBooking.StadiumId)
            {
                if (item.StartTime >= startTime || item.EndTime <= endTime)
                {
                    throw new Exception("This stadium is already booked in this time");
                }
            }
        }
        
        existBooking.StartTime = startTime;
        existBooking.EndTime = endTime;
    }

    public BookingViewModel Get(int id)
    {
        var existBooking = bookings.Find(x => x.Id == bookingId);

        if (existBooking == null)
        {
            throw new Exception("Booking is not found");
        }

        return existBooking;
    }

    public List<Booking> GetAll()
    {
        return bookings;
    }

    public List<Booking> GetAllByUserId(int userId)
    {        
        var result = new List<Booking>();
        
        foreach (var booking in bookings)
        {
            if (booking.UserId == userId)
            {
                result.Add(booking);
            }
        }
        
        return result;
    }

    public List<Booking> GetAllByStadiumId(int stadiumId)
    {
        var stadiumBookings = new List<Booking>();

        foreach (var item in bookings)
        {
            if (item.StadiumId == stadiumId)
            {
                stadiumBookings.Add(item);
            }
        }

        return stadiumBookings;
    }
}