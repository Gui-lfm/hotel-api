using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        private readonly TokenGenerator _tokenGenerator;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
            _tokenGenerator = new TokenGenerator();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                var user = _repository.Login(login);
                var token = _tokenGenerator.Generate(user);
                return Ok(new { token });
            }
            catch (Exception e)
            {

                return Unauthorized(new { message = e.Message });
            }
        }
    }
}