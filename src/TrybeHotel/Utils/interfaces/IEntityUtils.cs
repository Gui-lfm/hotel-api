using TrybeHotel.Dto;
using TrybeHotel.Models;

namespace TrybeHotel.Utils.interfaces;
public interface IEntityUtils
{
  City VerifyCity(int CityId);
  Hotel VerifyHotel(int HotelId);
  Room VerifyRoom(int RoomId);
  User VerifyUser(int UserId);
  Booking VerifyBooking(int BookingId);
  HotelDto CreateHotelDto(Hotel hotel);
  RoomDto CreateRoomDto(Room room, HotelDto hotel);
  UserDto CreateUserDto(User user);
  BookingResponse CreateBookingDto(Booking booking, RoomDto room);
}