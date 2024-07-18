using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        protected readonly IEntityUtils _entityUtils;
        public RoomRepository(ITrybeHotelContext context, IEntityUtils entityUtils)
        {
            _context = context;
            _entityUtils = entityUtils;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var hotel = _entityUtils.VerifyHotel(HotelId);
            var city = _entityUtils.VerifyCity(hotel.CityId);

            hotel.City = city;

            HotelDto hotelDto = _entityUtils.CreateHotelDto(hotel);

            var rooms = _context.Rooms.Where(r => r.HotelId == HotelId);

            var response = rooms.Select(room => _entityUtils.CreateRoomDto(room, hotelDto));

            return response;

        }

        public RoomDto AddRoom(Room room)
        {
            var hotel = _entityUtils.VerifyHotel(room.HotelId);
            var city = _entityUtils.VerifyCity(hotel.CityId);

            hotel.City = city;

            HotelDto hotelDto = _entityUtils.CreateHotelDto(hotel);

            _context.Rooms.Add(room);
            _context.SaveChanges();

            var createdDto = _entityUtils.CreateRoomDto(room, hotelDto);

            return createdDto;
        }

        public void DeleteRoom(int RoomId)
        {
            var room = _entityUtils.VerifyRoom(RoomId);

            _context.Rooms.Remove(room);
            _context.SaveChanges();
        }
    }
}