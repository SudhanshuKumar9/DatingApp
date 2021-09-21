using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.UserRepository
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public UserRepo(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AppUser> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserDto> AddUserAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<bool> IsAvailableUsername(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

    }
}
