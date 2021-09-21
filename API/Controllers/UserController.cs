using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.UserRepository;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepo _userRepo;

        public UsersController(IUserRepo user)
        {
            _userRepo = user;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<AppUser> GetUser(int id)
        {
            return await _userRepo.GetUserAsync(id);
        }


        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _userRepo.GetUsersAsync();
        }
    }
}
