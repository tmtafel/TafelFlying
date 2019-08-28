using System.ComponentModel.DataAnnotations;

namespace TafelFlyingServices.Models
{
    public class Aircraft
    {
        [Key]
        [Required]
        public int AircraftId { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        public double? Length { get; set; }

        public double? Wingspan { get; set; }

        public double? Height { get; set; }

        [Display(Name = "Wing Area")]
        public double? WingArea { get; set; }

        [Display(Name = "Aspect Ratio")]
        public string AspectRation { get; set; }

        [Display(Name = "Empty Weight")]
        public double? EmptyWeight { get; set; }

        [Display(Name = "Max Takeoff Weight")]
        public double? MaxTakeoffWeight { get; set; }

        public string Powerplant { get; set; }
    }
}