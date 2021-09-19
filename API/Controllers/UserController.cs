using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.UserRepository;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UsersController(IUserRepo user)
        {
            _userRepo = user;
        }

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
