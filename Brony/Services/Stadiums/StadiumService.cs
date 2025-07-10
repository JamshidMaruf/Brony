using Brony.Domain;
using Brony.Helpers;
using Brony.Services.Bookings;

namespace Brony.Services.Stadiums;

public class StadiumService : IStadiumService
{
    private int stadiumId;
    private readonly List<Stadium> stadiums;
    private readonly ObjectHolder objectHolder;

    public StadiumService(ObjectHolder objectHolder)
    {
        this.objectHolder = objectHolder;
        stadiumId = 1;
        stadiums = new List<Stadium>();
    }

    public void Create(
        string name,
        float width,
        float length,
        decimal price,
        string location,
        string phoneNumber,
        string description)
    {
        var existingStadium = stadiums.Find(x => x.Name == name);
        if (existingStadium != null)
        {
            throw new Exception("Stadium already exists");
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            throw new Exception("Phone should not be null or empty");
        }

        if (phoneNumber.Length != 12)
        {
            throw new Exception("Phone number should be 12 characters");
        }

        if (!phoneNumber.StartsWith("+998"))
        {
            throw new Exception("Phone number should start with '+998'");
        }

        var stadium = new Stadium
        {
            Id= IdGeneration.IdGenerate(Constants.PathHolder.StadiumIdPath),
            Name = name,
            Width = width,
            Length = length,
            Price = price,
            Location = location,
            PhoneNumber = phoneNumber,
            Description = description
        };

        stadiums.Add(stadium);
    }

    public void Update(
        int id,
        string name,
        float width,
        float length,
        decimal price,
        string location,
        string phoneNumber,
        string description)
    {
        var existStadium = stadiums.Find(x => x.Id == id);

        if (existStadium == null)
        {
            throw new Exception("Stadium is not found");
        }

        var alreadyExistStadium = stadiums.Find(x => x.Name == name);

        if (alreadyExistStadium != null)
        {
            throw new Exception($"Stadium already exists with this name = {name}");
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            throw new Exception("Phone should not be null or empty");
        }

        if (phoneNumber.Length != 12)
        {
            throw new Exception("Phone number should be 12 characters");
        }

        if (!phoneNumber.StartsWith("+998"))
        {
            throw new Exception("Phone number should start with '+998'");
        }

        existStadium.Name = name;
        existStadium.Location = location;
        existStadium.Length = length;
        existStadium.Width = width;
        existStadium.PhoneNumber = phoneNumber;
        existStadium.Price = price;
        existStadium.Description = description;
    }

    public void Delete(int id)
    {
        var existStadium = stadiums.Find(x => x.Id == id);

        if (existStadium != null)
        {
            throw new Exception("Stadium is not found");
        }

        stadiums.Remove(existStadium);
    }

    public Stadium Get(int id)
    {
        var existStadium = stadiums.Find(x => x.Id == id);

        if (existStadium != null)
        {
            throw new Exception("Stadium is not found");
        }

        return existStadium;
    }

    public List<Stadium> GetAll()
    {
        return stadiums;
    }

    public List<Stadium> Search(string search)
    {
        var result = new List<Stadium>();

        if (!string.IsNullOrEmpty(search))
        {
            string trimedString = search.TrimStart(' ').ToLower();

            foreach (var stadium in stadiums)
            {
                if (stadium.Name.ToLower().Contains(trimedString))
                {
                    result.Add(stadium);
                }
            }
        }

        return result;
    }

    public List<Stadium> GetFilteredList(
        string location,
        decimal? price,
        DateTime? startTime,
        DateTime? endTime)
    {
        var result = new List<Stadium>();

        var locationResult = new List<Stadium>();
        var priceResult = new List<Stadium>();
        var dateTimeResult = new List<Stadium>();

        if (!string.IsNullOrEmpty(location))
        {
            locationResult = GetAllByLocation(location);

            result = locationResult;
        }

        if (price != null)
        {
            if (locationResult.Any())
            {
                priceResult = GetAllByPrice(locationResult, Convert.ToDecimal(price));

                result = priceResult;
            }
            else
            {
                priceResult = GetAllByPrice(stadiums, Convert.ToDecimal(price));

                result = priceResult;
            }
        }

        if (startTime != null && endTime != null)
        {
            if (priceResult.Any())
            {
                dateTimeResult = GetAllByDateTime(
                    priceResult,
                    Convert.ToDateTime(startTime),
                    Convert.ToDateTime(endTime));

                result = dateTimeResult;
            }
            else if (locationResult.Any())
            {
                dateTimeResult = GetAllByDateTime(
                    locationResult,
                    Convert.ToDateTime(startTime),
                    Convert.ToDateTime(endTime));

                result = dateTimeResult;
            }
            else
            {
                dateTimeResult = GetAllByDateTime(
                    stadiums,
                    Convert.ToDateTime(startTime),
                    Convert.ToDateTime(endTime));

                result = dateTimeResult;
            }
        }

        return result;
    }
    
    private List<Stadium> GetAllByLocation(string location)
    {
        var result = new List<Stadium>();

        foreach (var stadium in stadiums)
        {
            if (stadium.Location.ToLower().Contains(location.ToLower()))
            {
                result.Add(stadium);
            }
        }

        return result;
    }

    private List<Stadium> GetAllByPrice(List<Stadium> stadiums, decimal price)
    {
        var result = new List<Stadium>();

        foreach (var stadium in stadiums)
        {
            if (stadium.Price == price)
            {
                result.Add(stadium);
            }
        }

        return result;
    }

    private List<Stadium> GetAllByDateTime(
        List<Stadium> stadiums,
        DateTime startTime,
        DateTime endTime)
    {
        var result = new List<Stadium>();

        foreach (var stadium in stadiums)
        {
            var startTimePeaces = stadium.StartWorkingTime.Split(':');
            var startTimeHour = Convert.ToInt32(startTimePeaces[0]);

            var endTimePeaces = stadium.EndWorkingTime.Split(':');
            var endTimeHour = Convert.ToInt32(endTimePeaces[0]);

            if (startTimeHour <= startTime.Hour || endTimeHour >= endTime.Hour)
            {
                var stadiumBookings = objectHolder.BookingService.GetAllByStadiumId(stadium.Id);

                foreach (var item in stadiumBookings)
                {
                    if (item.StartTime != startTime || item.EndTime != endTime)
                    {
                        result.Add(stadium);
                    }
                }
            }
        }
        
        return result;
    }
}