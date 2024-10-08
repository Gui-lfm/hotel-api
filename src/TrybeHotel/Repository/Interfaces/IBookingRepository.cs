using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository.Interfaces
{
    public interface IBookingRepository
    {
        BookingResponse Add(BookingDtoInsert booking, string email);
        Room GetRoomById(int RoomId);
        BookingResponse GetBooking(int bookingId, string email);
    }
}