using System.Reflection.Emit;
using Brony.Constants;
using Brony.Domain;
using Brony.Extensions;
using Brony.Helpers;
using Brony.Models;
using Brony.Services.Stadiums;
using Brony.Services.Users;

namespace Brony.Services.Bookings;

public class BookingService : IBookingService
{
    private int bookingId;
    private readonly UserService userService;
    private readonly StadiumService stadiumService;

    public BookingService(UserService userService, StadiumService stadiumService)
    {
        bookingId = 1;
        this.userService = userService;
        this.stadiumService = stadiumService;
    }

    public void Book(BookingCreateModel createModel)
    {
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        // check user
 
        // check stadium
        var existStadium = stadiumService.Get(createModel.StadiumId);

        // stadium working hours
        TimeSpan startTimeOfStadium = TimeSpan.Parse(existStadium.StartWorkingTime);
        TimeSpan endTimeOfStadium = TimeSpan.Parse(existStadium.EndWorkingTime);

        //booking start and end time
        TimeSpan startTimeOfMatch = createModel.StartTime.TimeOfDay;
        TimeSpan endTimeOfMatch = createModel.EndTime.TimeOfDay;


        if (startTimeOfStadium > startTimeOfMatch || endTimeOfStadium < endTimeOfMatch)
        {
            throw new Exception($"Stadium does not work in this time period! | " +
                                $"Working time: {existStadium.StartWorkingTime} - {existStadium.EndWorkingTime}");
        }

        // check time which is not booked
        foreach (var item in convertedBookings)
        {
            if (item.StadiumId == createModel.StadiumId)
            {
                if (item.StartTime < createModel.EndTime && item.EndTime > createModel.StartTime)
                {
                    throw new Exception("This stadium is already booked in this time");
                }
            }
        }

        // calculating the total price
        double numberOfMatchHours = (endTimeOfMatch - startTimeOfMatch).TotalHours;
        decimal totalPrice = (decimal)numberOfMatchHours * existStadium.Price;

        // preparing for fileload
        string content =
            $"{bookingId},{createModel.UserId},{createModel.StadiumId}," +
            $"{createModel.StartTime},{createModel.EndTime},{totalPrice}\n";

        File.AppendAllText(PathHolder.BookingsFilePath, content);

        bookingId++;
    }

    public void Cancel(int bookingId)
    {
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        var existBooking = convertedBookings.Find(x => x.Id == bookingId);

        if (existBooking == null)
        {
            throw new Exception("Booking was not found");
        }

        convertedBookings.Remove(existBooking);

        List<string> bookingsInStringFormat = FileHelper.ToFileFormat<Booking>(convertedBookings);
        File.WriteAllLines(PathHolder.BookingsFilePath, bookingsInStringFormat);
    }

    public void ChangeDateTime(int bookingId, DateTime startTime, DateTime endTime)
    {
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        var existBooking = convertedBookings.Find(x => x.Id == bookingId);

        if (existBooking == null)
        {
            throw new Exception("Booking was not found");
        }

        var existStadium = stadiumService.Get(existBooking.StadiumId);

        // stadium working hours
        TimeSpan startTimeOfStadium = TimeSpan.Parse(existStadium.StartWorkingTime);
        TimeSpan endTimeOfStadium = TimeSpan.Parse(existStadium.EndWorkingTime);

        //booking start and end time
        TimeSpan startTimeOfMatch = startTime.TimeOfDay;
        TimeSpan endTimeOfMatch = endTime.TimeOfDay;


        if (startTimeOfStadium > startTimeOfMatch || endTimeOfStadium < endTimeOfMatch)
        {
            throw new Exception($"Stadium does not work in this time period! | " +
                                $"Working time: {existStadium.StartWorkingTime} - {existStadium.EndWorkingTime}");
        }

        // check time which is not booked
        foreach (var booking in convertedBookings)
        {
            if (booking.StadiumId == existBooking.StadiumId)
            {
                if (booking.StartTime < endTime && booking.EndTime > startTime)
                {
                    throw new Exception("This stadium is already booked in this time");
                }
            }
        }


        existBooking.StartTime = startTime;
        existBooking.EndTime = endTime;

        List<string> bookingsInStringFormat = FileHelper.ToFileFormat<Booking>(convertedBookings);
        File.WriteAllLines(PathHolder.BookingsFilePath, bookingsInStringFormat);
    }

    public Booking Get(int id)
    {
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        var existBooking = convertedBookings.Find(x => x.Id == id);

        if (existBooking == null)
        {
            throw new Exception("Booking is not found");
        };
        return existBooking;
    }

 

    public List<Booking> GetAll()
    {
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        return convertedBookings;
    }

    public List<Booking> GetAllByUserId(int userId)
    {
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        var result = new List<Booking>();

        foreach (var booking in convertedBookings)
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
        string text = FileHelper.ReadFromFile(PathHolder.BookingsFilePath);

        List<Booking> convertedBookings = text.ToBooking();

        var stadiumBookings = new List<Booking>();

        foreach (var item in convertedBookings)
        {
            if (item.StadiumId == stadiumId)
            {
                stadiumBookings.Add(item);
            }
        }

        return stadiumBookings;
    }

}