using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils.interfaces;

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

        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels.ToList();

            var DtoResponse = hotels.Select(hotel =>
            {
                var city = _entityUtils.VerifyCity(hotel.CityId);
                hotel.City = city;
                return _entityUtils.CreateHotelDto(hotel);
            });

            return DtoResponse;
        }

        public HotelDto AddHotel(Hotel hotel)
        {
            var cityExists = _entityUtils.VerifyCity(hotel.CityId);

            hotel.City = cityExists;

            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var createdDto = _entityUtils.CreateHotelDto(hotel);

            return createdDto;
        }
    }
}