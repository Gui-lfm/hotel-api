using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository.Interfaces
{
    public interface ICityRepository
    {
        IEnumerable<CityDto> GetCities();
        CityDto AddCity(CityDtoInsert city);
        CityDto UpdateCity(CityDto city);
    }
}