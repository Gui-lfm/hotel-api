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
    [Route("hotel")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _repository;

        public HotelController(IHotelRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista os hotéis presentes no banco de dados.
        /// </summary>
        /// <response code="200">Retorna uma lista de objetos HotelDto.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpGet]
        public IActionResult GetHotels()
        {
            try
            {
                var response = _repository.GetHotels().ToList();

                return Ok(response);
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Adiciona um novo hotel ao banco de dados. Autorização de 'Admin' necessária para a operação.
        /// </summary>
        /// <param name="hotel">Objeto HotelDtoInsert contendo o nome, endereço e cityId:</param>
        /// <response code="201"> Retorna os itens do objeto HotelDto.</response>
        /// <response code="400"> Se o corpo da requisição estiver inválido.</response>
        /// <response code="401"> Se o usuário não possuir a autorização necessária ou caso esteja inválida.</response>
        /// <response code="404"> Se a entidade referenciada não for encontrada.</response>
        /// <response code="500"> Se ocorrer um erro interno do servidor.</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult PostHotel([FromBody] HotelDtoInsert hotel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _repository.AddHotel(hotel);

                return Created("", response);
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