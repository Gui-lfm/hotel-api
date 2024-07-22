using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Dto;
using TrybeHotel.Services;


namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("geo")]
    public class GeoController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly IGeoService _geoService;


        public GeoController(IHotelRepository repository, IGeoService geoService)
        {
            _repository = repository;
            _geoService = geoService;
        }

        /// <summary>
        /// Retorna o status da api externa.
        /// </summary>
        /// <returns> Retorna um objeto de resposta com o status da api.</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso.</response>
        /// <response code="401">Caso não seja obtida resposta da api externa.</response>
        /// <response code="500">Se ocorrer um erro interno do servidor.</response>
        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetStatus()
        {
            var status = await _geoService.GetGeoStatus();

            if (status == null)
            {
                return BadRequest();
            }

            return Ok(status);
        }

        [HttpGet]
        [Route("address")]
        public async Task<IActionResult> GetHotelsByLocation([FromBody] GeoDto address)
        {
            var response = await _geoService.GetHotelsByGeo(address, _repository);
            return Ok(response);
        }
    }


}