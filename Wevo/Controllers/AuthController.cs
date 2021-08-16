using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wevo.Models;
using Wevo.Repositories;

namespace Wevo.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(AppDbContext context)
        {
            _userRepository = new UserRepository(context);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] User model)
        {
            if (model.email == null || model.password == null)
            {
                return NotFound(new { message = "Email and Password Is Required" });
            }

            var login = _userRepository.Login(model.email, model.password);
            if (login.user == null)
            {
                return NotFound(new { message = "User or Password Invalid" });
            }

            return new
            {
                user = login.user,
                token = login.token
            };
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Register([FromBody] User model)
        {
            var validate = _userRepository.ValidateRegister(model);
            if (validate.error == true)
            {
                return UnprocessableEntity(new { message = validate.message });
            }

            var user = await _userRepository.Register(model);
            var login = _userRepository.Login(user.email, user.password);

            return new
            {
               user = login.user,
               token = login.token
            };
        }

    }
}
