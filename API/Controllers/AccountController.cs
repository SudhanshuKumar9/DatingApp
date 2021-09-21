using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data.UserRepository;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly IUserRepo _userRepo;
        private readonly ITokenService _tokenService;

        public AccountController(IUserRepo userRepo, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {

            if ( await IsUsernameExist(register.Username)) { return BadRequest("Usernamme already taken"); }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = register.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key
            };

            return await _userRepo.AddUserAsync(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginRequest)
        {
            var user = await _userRepo.GetUserAsync(loginRequest.Username);

            if(user == null) { return Unauthorized("Invalid usernamme"); }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            for(int i=0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        private async Task<bool> IsUsernameExist(string username)
        {
            return await _userRepo.IsAvailableUsername(username.ToLower());
        }
    }
}
