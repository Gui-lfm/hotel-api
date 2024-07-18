using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

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

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (email == null)
                {
                    throw new Exception("valid user email is required");
                }

                var response = _repository.Add(bookingInsert, email);
                return Created("", response);
            }
            catch (Exception e)
            {

                return Conflict(new { message = e.Message });
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    throw new UnauthorizedAccessException();
                }
                var response = _repository.GetBooking(Bookingid, email);
                return Ok(response);
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException)
                {
                    return Unauthorized(new { message = e.Message });
                }
                return Conflict(new { message = e.Message });
            }
        }
    }
}