using Brony.Constants;
using Brony.Domain;
using Brony.Extensions;
using Brony.Helpers;
using Brony.Models;

namespace Brony.Services.Users;

public class UserService : IUserService
{
    public UserService()
    {
    }

    public void Register(UserRegisterModel model)
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

        var existUser = users.Find(u => u.PhoneNumber == model.PhoneNumber);
        if (existUser != null)
        {
            throw new Exception("User with this phone number already exists.");
        }

        users.Add(model.ToConvert<>());

        var convertedUser = users.ToFileFormat();

        FileHelper.WriteToFile(PathHolder.UsersFilePath, convertedUser);
    }


    public int Login(string phoneNumber, string password)
    {
        var existUser = users.Find(u => u.PhoneNumber == phoneNumber);
        if (existUser == null)
        {
            throw new Exception("Phone or password is incorrect.");
        }

        if (existUser.Password != password)
        {
            throw new Exception("Phone or password is incorrect.");
        }

        return existUser.Id;
    }


    public UserRegisterModel Get(int id)
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();
        var existUser = users.Find(u => u.Id == id);
        if (existUser == null)
        {
            throw new Exception("User is not found.");
        }

        return existUser.ToConvert<UserRegisterModel>();
    }


    public void Update(
        int id,
        string firstName,
        string lastName,
        string phoneNumber)
    {
        var existUser = users.Find(u => u.Id == id);
        if (existUser == null)
        {
            throw new Exception("Phone is not found.");
        }

        var alreadyExistUser = users.Find(u => u.PhoneNumber == phoneNumber);
        if (alreadyExistUser != null)
        {
            throw new Exception("User already exists with this phone number.");
        }

        existUser.PhoneNumber = phoneNumber;
        existUser.LastName = lastName;
        existUser.FirsName = firstName;
    }

    public void Delete(int id)
    {
        var existUser = users.Find(u => u.Id == id);
        if (existUser == null)
        {
            throw new Exception("Phone is not found.");
        }

        users.Remove(existUser);
    }

    public List<User> GetAll()
    {
        return users;
    }

    public List<User> Search(string search)
    {
        var result = new List<User>();

        if (!string.IsNullOrEmpty(search))
        {
            foreach (var user in users)
            {
                if (
                    user.FirsName.ToLower().Contains(search.ToLower()) ||
                    user.LastName.ToLower().Contains(search.ToLower()) ||
                    user.PhoneNumber.ToLower().Contains(search.ToLower()))
                {
                    result.Add(user);
                }
            }
        }

        return result;
    }

    public void ChangePassword(int userId, string oldPassword, string newPassword)
    {
        var existUser = users.Find(u => u.Id == userId);
        if (existUser == null)
            throw new Exception("User is not found.");

        if (existUser.Password != oldPassword)
            throw new Exception("Passwords do not match.");

        existUser.Password = newPassword;
    }
}