using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using Shared_Guesser.Helpers;
using System.Threading.Tasks;

namespace DataLayer_Guesser.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<PaginatedData<User>> GetUsers(UserConfigs config);
        Task<User> CreateUser(User userToCreate);
        Task<User> UpdateUser(User userToUpdate);
        Task<bool> UpdateUserPassword(int userId, string newPassword, string userDetails);
        Task<bool> UpdateUserDetails(int userId, string userDetails);
        Task<bool> DeleteUser(int userId);
        Task<bool> SendMessage(string email, MessageTypes messageType);
        Task<bool> UpdateUserStatus(int userId, UserStatus newStatus);
    }
}
