using System.Collections.Generic;
using System.Threading.Tasks;
using backEnd.Models;
using backEnd.Models.DTOs;

namespace backEnd.Services.IServices
{
    public interface IUsersService
    {
        Task<List<User>> GetAsync();
        Task<List<User>> GetUsersForSupervisor(int id);
        Task<User> GetAuditor();
        Task<User> GetUserByMail(string mail);
        Task<User> GetUserByName(string name);
        Task<List<User>> GetUsersByMail(List<string> mails);
        Task<User?> GetUserByMailAndPassword(string mail, string password);
        Task<List<User>> GetAllNormalUsers();
        Task<List<User>> GetUsersIncludingAdmin();
        Task<User?> GetOneUser(int? id);
        Task CreateAsync(User newUser);
        Task UpdateAsync(int id, User updatedTicket);
        Task UpdateUserNumber(User user);
        Task RemoveAsync(int id);
        Task<List<string>> GetUserEmails();
    }
}
