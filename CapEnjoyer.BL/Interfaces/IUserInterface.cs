namespace CapEnjoyer.BL.Interfaces;

using DAL.Entities;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(Guid id, User user);
    Task DeleteUser(Guid id);
}
