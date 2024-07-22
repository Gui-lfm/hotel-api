using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retorna uma lista de usuários registrados no banco de dados. Autorização de 'Admin' necessária para a operação.
        /// </summary>
        /// <returns>
        /// Lista de objetos UserDto, contendo userId, name, email e userType
        /// </returns>
        /// <response code="200"> Retorna lista de usuários presentes no banco de dados.</response>
        /// <response code="401"> Se o usuário não possuir a autorização necessária ou caso esteja inválida.</response>
        /// <response code="500"> Se ocorrer um erro interno do servidor.</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            try
            {
                var response = _repository.GetUsers();

                return Ok(response);
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        ///  Adiciona um usuário ao banco de dados
        /// </summary>
        /// <param name="user">Objeto UserDtoInsert, contendo nome, email e senha</param>
        /// <returns>Objeto UserDto, contendo userId, nome, email, senha e userType. Por padrão todo usuário é registrado com userType 'client'</returns>
        /// <response code="201"> Retorna objeto UserDto</response>
        /// <response code="409"> se o email a ser registrado já existe no banco de dados.</response>
        /// <response code="500"> Se ocorrer um erro interno do servidor.</response>
        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            try
            {
                var response = _repository.Add(user);
                return Created("", response);
            }
            catch (EmailAlreadyRegisteredException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}