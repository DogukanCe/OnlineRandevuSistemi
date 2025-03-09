using Microsoft.EntityFrameworkCore;
using OnlineRandevuSistemi.Entites.UserToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRandevuSistemi.DataAccess
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
      
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(User user);
       
    }
}
