using LMS.DataAccess.Models;

namespace LMS.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsExistsAsync(string username);
        Task<User?> GetUserAsync(string username, string password);
        Task RegisterAsync(User user);
    }
}
