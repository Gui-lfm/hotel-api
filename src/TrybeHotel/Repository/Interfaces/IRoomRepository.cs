using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<RoomDto> GetRooms(int HotelId);
        RoomDto AddRoom(RoomDtoInsert room);

        void DeleteRoom(int RoomId);
    }
}