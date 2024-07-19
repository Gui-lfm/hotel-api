using System.Net.Http;
using TrybeHotel.Dto;
using TrybeHotel.Repository.Interfaces;

namespace TrybeHotel.Services
{
    public class GeoService : IGeoService
    {
        private readonly HttpClient _client;
        private const string _baseUrl = "https://nominatim.openstreetmap.org/";
        public GeoService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<object> GetGeoStatus()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "status?format=json");

            req.Headers.Add("Accept", "application/json");
            req.Headers.Add("User-Agent", "aspnet-user-agent");

            var res = await _client.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {
                var status = await res.Content.ReadFromJsonAsync<object>();
                return status!;
            }
            else
            {
                return default!;
            }
        }

        public async Task<GeoDtoResponse> GetGeoLocation(GeoDto geoDto)
        {
            var req = new HttpRequestMessage(
             HttpMethod.Get,
             $"search?street={geoDto.Address}&city={geoDto.City}&country=Brazil&state={geoDto.State}&format=json&limit=1");
            req.Headers.Add("Accept", "application/json");
            req.Headers.Add("User-Agent", "aspnet-user-agent");

            var res = await _client.SendAsync(req);

            if (!res.IsSuccessStatusCode)
            {
                return default(GeoDtoResponse)!;
            }

            var location = await res.Content.ReadFromJsonAsync<List<GeoDtoResponse>>();

            return new GeoDtoResponse
            {
                lat = location![0].lat,
                lon = location![0].lon
            };
        }

        public async Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository)
        {

            var hotelList = repository.GetHotels();

            if (hotelList == null)
            {
                throw new InvalidOperationException("No hotels found.");
            }

            var originLocation = await GetGeoLocation(geoDto);

            if (originLocation == null)
            {
                return new List<GeoDtoHotelResponse>();
            }

            List<GeoDtoHotelResponse> response = new();

            foreach (var hotel in hotelList)
            {

                var hotelLocation = await GetGeoLocation(new GeoDto
                {
                    Address = hotel.Address,
                    City = hotel.CityName,
                    State = hotel.State
                });

                var distance = CalculateDistance(
                    originLocation.lat!,
                    originLocation.lon!,
                    hotelLocation.lat!,
                    hotelLocation.lon!
                );

                response.Add(new GeoDtoHotelResponse
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityName = hotel.CityName,
                    State = hotel.State,
                    Distance = distance
                });
            }

            return response.OrderBy(hotel => hotel.Distance).ToList();
        }



        public int CalculateDistance(string latitudeOrigin, string longitudeOrigin, string latitudeDestiny, string longitudeDestiny)
        {
            double latOrigin = double.Parse(latitudeOrigin.Replace('.', ','));
            double lonOrigin = double.Parse(longitudeOrigin.Replace('.', ','));
            double latDestiny = double.Parse(latitudeDestiny.Replace('.', ','));
            double lonDestiny = double.Parse(longitudeDestiny.Replace('.', ','));
            double R = 6371;
            double dLat = radiano(latDestiny - latOrigin);
            double dLon = radiano(lonDestiny - lonOrigin);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(radiano(latOrigin)) * Math.Cos(radiano(latDestiny)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return int.Parse(Math.Round(distance, 0).ToString());
        }

        public double radiano(double degree)
        {
            return degree * Math.PI / 180;
        }

    }
}