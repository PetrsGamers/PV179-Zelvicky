namespace CapEnjoyer.DAL.Seeds;

using System.Security.Cryptography;
using System.Text;
using Entities;
using Exceptions;

public static class SeedUtils
{
    private static int GenerateUniqueInt(string key)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return BitConverter.ToInt32(hashBytes, 0);
    }

    public static Random GetRandom(string seed) => new(GenerateUniqueInt(seed));

    public static Guid GetUserId(string username, List<User> users)
    {
        var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new NotFoundException($"User with username '{username}' not found.");

        return user.Id;
    }
}
