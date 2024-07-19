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

        public CityDto AddCity(CityDtoInsert city)
        {
            var newCity = new City
            {
                Name = city.Name!,
                State = city.State!
            };

            _context.Cities.Add(newCity);
            _context.SaveChanges();

            var response = new CityDto
            {
                CityId = newCity.CityId,
                Name = newCity.Name,
                State = newCity.State
            };

            return response;
        }

        public CityDto UpdateCity(CityDto city)
        {
            var cityFound = _entityUtils.VerifyCity(city.CityId);
            cityFound.Name = city.Name!;
            cityFound.State = city.State!;

            _context.Cities.Update(cityFound);
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