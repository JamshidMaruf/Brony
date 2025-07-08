using Brony.Domain;
using Brony.Models;
namespace Brony.Extensions;
public static class ConvertTo
{
    public static UserRegisterModel ConvertingTo(this User obj)
    {
        if (obj == null) 
        {
            throw new Exception("Object is null here");
        }
            
        return new UserRegisterModel
        {
            FirstName = obj.FirstName,
            LastName = obj.LastName,
            PhoneNumber = obj.PhoneNumber
        };
    }
}
