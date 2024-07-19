using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Context.Interfaces;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        protected readonly IEntityUtils _entityUtils;
        public CityRepository(ITrybeHotelContext context, IEntityUtils entityUtils)
        {
            _context = context;
            _entityUtils = entityUtils;
        }

        public IEnumerable<CityDto> GetCities()
        {
            var response = _context.Cities.Select(city => new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            });

            return response;
        }

        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

            var response = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };

            return response;
        }

        public CityDto UpdateCity(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();

            var response = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };

            return response;
        }

    }
}