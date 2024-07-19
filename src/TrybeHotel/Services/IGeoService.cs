using TrybeHotel.Dto;
using TrybeHotel.Repository.Interfaces;

namespace TrybeHotel.Services
{
    public interface IGeoService
    {
        Task<object> GetGeoStatus();
        Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository);
    }
}