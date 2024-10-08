using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Context.Interfaces;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        private readonly IEntityUtils _entityUtils;
        public HotelRepository(ITrybeHotelContext context, IEntityUtils entityUtils)
        {
            _context = context;
            _entityUtils = entityUtils;
        }

        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
            _entityUtils = null!;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            var DtoResponse = _context.Hotels
            .Join(
                _context.Cities,
                hotel => hotel.CityId,
                city => city.CityId,
                (hotel, city) => new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = hotel.CityId,
                    CityName = city.Name,
                    State = city.State
                });

            return DtoResponse.ToList();
        }

        public HotelDto AddHotel(HotelDtoInsert hotelDto)
        {
            var cityExists = _entityUtils.VerifyCity(hotelDto.CityId);

            var newHotel = new Hotel
            {
                Name = hotelDto.Name!,
                Address = hotelDto.Address!,
                CityId = hotelDto.CityId,
                City = cityExists
            };

            _context.Hotels.Add(newHotel);
            _context.SaveChanges();

            var createdDto = _entityUtils.CreateHotelDto(newHotel);

            return createdDto;
        }
    }
}