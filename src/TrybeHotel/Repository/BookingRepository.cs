using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Exceptions;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Context.Interfaces;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        protected readonly IUserRepository _userRepository;
        protected readonly IEntityUtils _entityUtils;
        public BookingRepository(
            ITrybeHotelContext context,
            IUserRepository userRepository,
            IEntityUtils entityUtils
            )
        {
            _context = context;
            _userRepository = userRepository;
            _entityUtils = entityUtils;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var user = _userRepository.GetUserByEmail(email);

            var totalGuests = booking.GuestQuant;
            var room = _entityUtils.VerifyRoom(booking.RoomId);

            if (totalGuests > room.Capacity)
            {
                throw new GuestOverCapacityException("Guest quantity over room capacity");
            }

            var hotel = _entityUtils.VerifyHotel(room.HotelId);
            var city = _entityUtils.VerifyCity(hotel.CityId);

            hotel.City = city;

            var hotelDto = _entityUtils.CreateHotelDto(hotel);

            var roomDto = _entityUtils.CreateRoomDto(room, hotelDto);

            var newBooking = new Booking()
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = user.UserId,
                RoomId = room.RoomId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            var response = _entityUtils.CreateBookingDto(newBooking, roomDto);

            return response;
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _userRepository.GetUserByEmail(email);

            var booking = _entityUtils.VerifyBooking(bookingId);

            if (user.UserId != booking.UserId)
            {
                throw new UnauthorizedAccessException("Unauthorized User");
            }

            var room = _entityUtils.VerifyRoom(booking.RoomId);
            var hotel = _entityUtils.VerifyHotel(room.HotelId);
            var city = _entityUtils.VerifyCity(hotel.CityId);

            hotel.City = city;

            var hotelDto = _entityUtils.CreateHotelDto(hotel);
            var roomDto = _entityUtils.CreateRoomDto(room, hotelDto);
            var response = _entityUtils.CreateBookingDto(booking, roomDto);

            return response;

        }

        public Room GetRoomById(int RoomId)
        {
            return _entityUtils.VerifyRoom(RoomId);
        }

    }

}