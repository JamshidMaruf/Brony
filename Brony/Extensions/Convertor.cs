using System.Reflection;
using Brony.Domain;

namespace Brony.Extensions;

public static class Convertor
{
    public static List<T> ToObject<T>(this string text)
    {
        return Enumerable.Range(0, text.Length)
            .Select(i => text[i])
            .Select(c => (T)Convert.ChangeType(c, typeof(T)))
            .ToList();
    }

    public static List<Booking> ToBooking(this string text)
    {
        List<Booking> bookings = new List<Booking>();

        string[] lines = text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            bookings.Add(new Booking
            {
                Id = int.Parse(parts[0]),
                UserId = int.Parse(parts[1]),
                StadiumId = int.Parse(parts[2]),
                StartTime = DateTime.Parse(parts[3]),
                EndTime = DateTime.Parse(parts[4]),
                Price = decimal.Parse(parts[5])
            });
        }

        return bookings;
    }

    
}
