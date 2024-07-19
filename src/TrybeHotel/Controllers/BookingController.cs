using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Adiciona uma nova reserva ao banco de dados.
        /// </summary>
        /// <param name="bookingInsert">Objeto BookingDtoInsert contendo os detalhes da reserva.</param>
        /// <returns>Os detalhes da reserva criado.</returns>
        /// <response code="201">Retorna os itens do objeto BookingResponse.</response>
        /// <response code="400">Se os dados fornecidos forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se a entidade referenciada não for encontrada.</response>
        /// <response code="409">Se o número de hospedes for maior que a capacidade do quarto.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                if (bookingInsert == null)
                {
                    return BadRequest(new { message = "Booking data is required" });
                }

                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    return Unauthorized(new { message = "Valid user email is required" });
                }

                var response = _repository.Add(bookingInsert, email);
                return Created("", response);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (GuestOverCapacityException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Endpoint responsável por listar uma única reserva.
        /// </summary>
        /// <param name="Bookingid">Id da reserva</param>
        /// <returns>Os detalhes da reserva especificada.</returns>
        /// <response code="200">Retorna os itens do objeto BookingResponse.</response>
        /// <response code="401">Se o usuário não estiver autenticado ou se as informações da reserva forem acessadas por um usuário que não a criou.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking([FromRoute] int Bookingid)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    return Unauthorized(new { message = "Valid user email is required" });
                }


                var response = _repository.GetBooking(Bookingid, email);
                return Ok(response);
            }
            catch (UnauthorizedAccessException e)
            {

                return Unauthorized(new { message = e.Message });
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }

        }
    }
}