using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
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

        /// <summary>
        /// Realiza o login de usuário existente no banco de dados
        /// </summary>
        /// <param name="login">Objeto loginDto, contendo email e senha</param>
        /// <returns>Retorna um token.</returns>
        /// <response code="200"> retorna um objeto com um token de autorização.</response>
        /// <response code="401"> Caso email e/ou senha estejam incorretos.</response>
        /// <response code="500"> Se ocorrer um erro interno do servidor.</response>
        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                var user = _repository.Login(login);
                var token = _tokenGenerator.Generate(user);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new { message = e.Message });
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}