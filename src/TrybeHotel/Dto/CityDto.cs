using System.ComponentModel.DataAnnotations;

namespace TrybeHotel.Dto
{
    public class CityDto
    {
        public int CityId { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
    }

    public class CityDtoInsert
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? State { get; set; }
    }
}