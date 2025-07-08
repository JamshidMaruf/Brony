using Brony.Domain;
using System.Xml.Linq;

namespace Brony.Extensions;
public static class AddStadium
{
    #region AddStadiums()
    public static void AddStadiums(this List<Stadium> stadiums, int stadiumId, string name, int width, int length, int price, string location, string phoneNumber, string description)
    {
        var newStadium = new Stadium()
        {
            Id = stadiumId,
            Name = name,
            Width = width,
            Length = length,
            Price = price,
            Location = location,
            PhoneNumber = phoneNumber,
            Description = description
        };
        stadiums.Add(newStadium);
    }
    #endregion
}
