using System.ComponentModel.DataAnnotations;

namespace TrybeHotel.Dto
{
     public class RoomDto
     {
          public int RoomId { get; set; }
          public string? Name { get; set; }
          public int Capacity { get; set; }
          public string? Image { get; set; }
          public HotelDto? Hotel { get; set; }
     }

     public class RoomDtoInsert
     {
          [Required]
          public string? Name { get; set; }
          [Required]
          public int Capacity { get; set; }
          [Required]
          public string? Image { get; set; }
          [Required]
          public int HotelId { get; set; }
     }
}