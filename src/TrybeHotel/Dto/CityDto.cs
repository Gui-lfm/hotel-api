using System.ComponentModel.DataAnnotations;

namespace TrybeHotel.Dto
{
    public class CityDto
    {
        [Required]
        public int CityId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
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