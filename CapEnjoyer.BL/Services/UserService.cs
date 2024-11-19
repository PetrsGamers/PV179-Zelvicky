namespace CapEnjoyer.BL.Services;

using BL.Interfaces;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class UserService(CapEnjoyerDbContext dbContext) : IUserService
{
    private readonly CapEnjoyerDbContext context = dbContext;

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await this.context.Users.ToListAsync();
        return users;
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await this.context.Users.FindAsync(id) ?? throw new ArgumentException($"User with this {id} not found.");

        return user;
    }

    public async Task<User> CreateUser(User user)
    {
        // Přidat logiku pro validaci nebo další procesy
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email))
        {
            throw new ArgumentException("Username and Email are required.");
        }
        await this.context.Users.AddAsync(user);
        await this.context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(Guid id, User user)
    {
        var existingUser = await this.context.Users.FindAsync(id) ?? throw new ArgumentException($"User with this {id} not found.");


        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Role = user.Role;


        this.context.Users.Update(existingUser);
        await this.context.SaveChangesAsync();
        return existingUser;
    }

    public async Task DeleteUser(Guid id)
    {
        var user = this.context.Users.Find(id) ?? throw new ArgumentException("User not found.");
        this.context.Users.Remove(user);
        await this.context.SaveChangesAsync();
    }
}
