using contact_management.Models;

namespace contact_management;

public interface IUserInterface
{
    Task<int> AddUser(User user);
    Task<User> GetUser(Login login);
}
