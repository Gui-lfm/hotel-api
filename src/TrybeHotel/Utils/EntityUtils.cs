using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Dto;

namespace TrybeHotel.Utils;

public class EntityUtils : IEntityUtils
{

  private readonly ITrybeHotelContext _context;

  public EntityUtils(ITrybeHotelContext context)
  {
    _context = context;
  }

  public City VerifyCity(int cityId)
  {
    var city = _context.Cities.FirstOrDefault(c => c.CityId == cityId) ?? throw new EntityNotFoundException("Cidade não existe no banco de dados");
    return city;
  }

  public Hotel VerifyHotel(int HotelId)
  {
    var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == HotelId) ?? throw new EntityNotFoundException($"Hotel com id {HotelId} não encontrado");
    return hotel;
  }

  public Room VerifyRoom(int RoomId)
  {
    var room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId) ?? throw new EntityNotFoundException($"Room/{RoomId} not found on database");
    return room;
  }

  public User VerifyUser(int UserId)
  {
    var user = _context.Users.FirstOrDefault(u => u.UserId == UserId) ?? throw new EntityNotFoundException("User not found on database");
    return user;
  }
  public Booking VerifyBooking(int BookingId)
  {
    var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == BookingId) ?? throw new EntityNotFoundException("Booking not found");
    return booking;
  }

  public HotelDto CreateHotelDto(Hotel hotel)
  {
    return new HotelDto()
    {
      HotelId = hotel.HotelId,
      Name = hotel.Name,
      Address = hotel.Address,
      CityId = hotel.CityId,
      CityName = hotel.City?.Name,
      State = hotel.City?.State
    };
  }

  public RoomDto CreateRoomDto(Room room, HotelDto hotel)
  {
    return new RoomDto()
    {
      RoomId = room.RoomId,
      Name = room.Name,
      Capacity = room.Capacity,
      Image = room.Image,
      Hotel = hotel
    };
  }

  public UserDto CreateUserDto(User user)
  {
    return new UserDto()
    {
      UserId = user.UserId,
      Name = user.Name,
      Email = user.Email,
      UserType = user.UserType
    };
  }

  public BookingResponse CreateBookingDto(Booking booking, RoomDto roomDto)
  {
    return new BookingResponse()
    {
      BookingId = booking.BookingId,
      CheckIn = booking.CheckIn,
      CheckOut = booking.CheckOut,
      GuestQuant = booking.GuestQuant,
      Room = roomDto
    };
  }
}