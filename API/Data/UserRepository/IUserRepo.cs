using API.DTOs;
using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.UserRepository
{
    public interface IUserRepo
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserAsync(int id);

        Task<AppUser> GetUserAsync(string username);

        Task<UserDto> AddUserAsync(AppUser user);

        Task<bool> IsAvailableUsername(string username);
    }
}
