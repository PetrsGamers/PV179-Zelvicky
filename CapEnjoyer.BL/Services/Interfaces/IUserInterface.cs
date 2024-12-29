namespace CapEnjoyer.BL.Services.Interfaces;

using DAL.Entities;

public interface IUserService
{
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> GetUserById(Guid id);
    public Task<User> CreateUser(User user);
    public Task<User> UpdateUser(Guid id, User user);
    public Task DeleteUser(Guid id);
    public Task<string?> GetUsernameById(Guid id);
}
