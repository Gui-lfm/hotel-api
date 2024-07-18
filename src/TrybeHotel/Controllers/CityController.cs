using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;

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

        [HttpGet]
        public IActionResult GetCities()
        {
            var response = _repository.GetCities().ToList();

            return Ok(response);
        }

        [HttpPost]
        public IActionResult PostCity([FromBody] City city)
        {

            var response = _repository.AddCity(city);

            return Created("", response);
        }

        // 3. Desenvolva o endpoint PUT /city
        [HttpPut]
        public IActionResult PutCity([FromBody] City city)
        {
            throw new NotImplementedException();
        }
    }
}