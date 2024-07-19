using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        [HttpGet]
        public IActionResult GetHotels()
        {
            var response = _repository.GetHotels().ToList();

            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult PostHotel([FromBody] Hotel hotel)
        {
            var response = _repository.AddHotel(hotel);

            return Created("", response);
        }


    }
}