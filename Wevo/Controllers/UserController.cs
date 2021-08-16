using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wevo.Models;
using Wevo.Repositories;

namespace Wevo.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(AppDbContext context)
        {
            _userRepository = new UserRepository(context);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userRepository.list();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}/update")]
        public async Task<dynamic> PutUser(int id, User user)
        {
            if (id.ToString() != User.Identity.Name)
            {
                return Forbid();
            }

            try
            {
                return await _userRepository.updateById(id, user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userRepository.userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.delete(user);

            return NoContent();
        }
    }
}
