namespace Brony.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public static bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}