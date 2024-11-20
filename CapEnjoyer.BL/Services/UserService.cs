namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserService(CapEnjoyerDbContext context) : IUserService
{
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await context.Users.ToListAsync();
        return users;
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await context.Users.FindAsync(id) ??
                   throw new ArgumentException($"User with this {id} not found.");

        return user;
    }

    public async Task<User> CreateUser(User user)
    {
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email))
        {
            throw new ArgumentException("Username and Email are required.");
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(Guid id, User user)
    {
        var existingUser = await context.Users.FindAsync(id) ??
                           throw new ArgumentException($"User with this {id} not found.");


        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Role = user.Role;


        context.Users.Update(existingUser);
        await context.SaveChangesAsync();
        return existingUser;
    }

    public async Task DeleteUser(Guid id)
    {
        var user = await context.Users.FindAsync(id) ?? throw new ArgumentException("User not found.");
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}
