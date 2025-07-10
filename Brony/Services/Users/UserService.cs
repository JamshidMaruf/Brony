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

        FileHelper.WriteToFile(PathHolder.UsersFilePath, users.ToFileFormat<User>());
    }


    public int Login(string phoneNumber, string password)
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

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


<<<<<<< HEAD
    public void Update(
        int id,
        string firstName,
        string lastName,
        string phoneNumber)
=======
    public void Update(UserUpdateModel model)
>>>>>>> c3d3f596d441d2dc79d9c76eb851231ffe05ba75
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

        var existUser = users.Find(u => u.Id == model.Id);
        if (existUser == null)
        {
            throw new Exception("User is not found.");
        }

<<<<<<< HEAD
        var alreadyExistUser = users.Find(u => u.PhoneNumber == phoneNumber);
=======
        var alreadyExistUser = users.Find(u => u.PhoneNumber == model.PhoneNumber);
>>>>>>> c3d3f596d441d2dc79d9c76eb851231ffe05ba75
        if (alreadyExistUser != null)
        {
            throw new Exception("User already exists with this phone number.");
        }

<<<<<<< HEAD
        existUser.PhoneNumber = phoneNumber;
        existUser.LastName = lastName;
        existUser.FirsName = firstName;
=======
        users.Add(model.ToConvert<UserUpdateModel>());

        var convertedUser = users.ToFileFormat();

        FileHelper.WriteToFile(PathHolder.UsersFilePath, convertedUser);
>>>>>>> c3d3f596d441d2dc79d9c76eb851231ffe05ba75
    }

    public void Delete(int id)
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

        var existUser = users.Find(u => u.Id == id);
        if (existUser == null)
        {
            throw new Exception("User is not found.");
        }

        users.Remove(existUser);

        var convertedUser = users.ToFileFormat<User>();

        FileHelper.WriteToFile(PathHolder.UsersFilePath, convertedUser);
    }

    public List<User> GetAll()
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

        return users;
    }

    public List<User> Search(string search)
    {
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

        var result = new List<User>();

        if (!string.IsNullOrEmpty(search))
        {
            foreach (var user in users)
            {
                if (
                    user.FirstName.ToLower().Contains(search.ToLower()) ||
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
        var text = FileHelper.ReadFromFile(PathHolder.UsersFilePath);

        var users = text.ToObject<User>();

        var existUser = users.Find(u => u.Id == userId);
        if (existUser == null)
            throw new Exception("User is not found.");

        if (existUser.Password != oldPassword)
            throw new Exception("Passwords do not match.");

        existUser.Password = newPassword;

        var convertedUser = users.ToFileFormat();

        FileHelper.WriteToFile(PathHolder.UsersFilePath, convertedUser);
    }
}
