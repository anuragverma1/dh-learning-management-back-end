using dh_learning_management_back_end.Models;
using dh_learning_management_back_end.Repository;
using Microsoft.AspNetCore.Mvc;

namespace dh_learning_management_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepository;

        public AuthController()
        {
            _authRepository = new AuthRepository();
        }

        [HttpPost("register")]
        public ActionResult<User> RegisterUser(UserRegisterDto request)
        {
            var response = _authRepository.RegisterUser(request);
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }

        [HttpPost("login")]
        public ActionResult<User> LoginUser(UserLoginDto request)
        {
            var response = _authRepository.Login(request);
            if (response.IsSuccess)
                return Ok(response.Message);

            return BadRequest(response.Message);
        }
    }
}