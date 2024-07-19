using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

                if (e is UnauthorizedAccessException)
                {
                    return Unauthorized(new { message = e.Message });
                }

                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            try
            {
                var response = _repository.Add(user);
                return Created("", response);
            }
            catch (Exception e)
            {

                return Conflict(new { message = e.Message });
            }
        }
    }
}