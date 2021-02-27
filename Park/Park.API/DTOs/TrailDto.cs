using System.ComponentModel.DataAnnotations;
using static Park.Core.Models.Trail;

namespace Park.API.DTOs
{
    public class TrailDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }
        public NationalParkDto NationalPark { get; set; }
    }
}
