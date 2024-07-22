using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///  Lista de quartos do hotel especificado pelo usuário. 
        /// </summary>
        /// <param name="HotelId">Id do hotel que deseja verificar os quartos</param>
        /// <returns> Retorna uma lista de objetos RoomDto do hotel especificado.</returns>
        /// <response code="200"> Em caso de sucesso, retorna uma lista com os quartos encotrados no banco de dados.</response>
        /// <response code="404"> Se a entidade referenciada não for encontrada.</response>
        /// <response code="500"> Se ocorrer um erro interno no servidor.</response>
        [HttpGet("{HotelId}")]
        public IActionResult GetRoom([FromRoute] int HotelId)
        {
            try
            {
                var response = _repository.GetRooms(HotelId);

                return Ok(response);
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

        /// <summary>
        /// Adiciona um novo quarto ao banco de dados. Autorização de 'Admin' necessária para a operação.
        /// </summary>
        /// <param name="room">Objeto roomDtoInsert contendo o nome, capacidade, imagem e hotelId:</param>
        /// <response code="201"> Retorna os itens do objeto RoomDto.</response>
        /// <response code="400"> Se o corpo da requisição estiver inválido.</response>
        /// <response code="401"> Se o usuário não possuir a autorização necessária ou caso esteja inválida.</response>
        /// <response code="404"> Se a entidade referenciada não for encontrada.</response>
        /// <response code="500"> Se ocorrer um erro interno do servidor.</response>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult PostRoom([FromBody] RoomDtoInsert room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _repository.AddRoom(room);

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
        /// <summary>
        /// Remove um quarto do banco de dados. Autorização de 'Admin' necessária para a operação.
        /// </summary>
        /// <param name="RoomId">Id do quarto que deseja remover.</param>
        /// <response code="204">Caso a operação seja bem sucedida.</response>
        /// <response code="401"> Se o usuário não possuir a autorização necessária ou caso esteja inválida.</response>
        /// <response code="500"> Se ocorrer um erro interno do servidor.</response>
        [HttpDelete("{RoomId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete([FromRoute] int RoomId)
        {
            try
            {
                _repository.DeleteRoom(RoomId);

                return NoContent();
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}