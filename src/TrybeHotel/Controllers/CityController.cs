using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("city")]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        public CityController(ICityRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista as cidades presentes no banco de dados.
        /// </summary>
        /// <response code="200">Retorna uma lista de objetos CityDto.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpGet]
        public IActionResult GetCities()
        {
            try
            {
                var response = _repository.GetCities().ToList();

                return Ok(response);
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Adiciona uma nova cidade ao banco de dados.
        /// </summary>
        /// <param name="city">Objeto CityDtoInsert contendo o nome e estado da cidade:</param>
        /// <response code="201">Retorna os itens do objeto CityDto.</response>
        /// <response code="400">Se os dados fornecidos forem inválidos.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpPost]
        public IActionResult PostCity([FromBody] CityDtoInsert city)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _repository.AddCity(city);

                return Created("", response);
            }
            catch (Exception e)
            {

                return StatusCode(500, new { message = e.Message });
            }
        }

        /// <summary>
        /// Atualiza as informações de uma cidade presente no banco de dados.
        /// </summary>
        /// <param name="city">Objeto City contendo id, nome e estado da cidade:</param>
        /// <response code="200">Retorna os itens do objeto CityDto.</response>
        /// <response code="400">Se os dados fornecidos forem inválidos.</response>
        /// <response code="404">Se a entidade referenciada não for encontrada.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpPut]
        public IActionResult PutCity([FromBody] CityDto city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = _repository.UpdateCity(city);

                return Ok(response);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
    }
}