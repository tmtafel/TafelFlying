using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Geocoding;

namespace TafelFlyingServices.Models
{
    public class Airport
    {
        [Key]
        [Display(Name = "Airport Code")]
        [Required]
        public string AirportId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Display(Name = "Region")]
        public string StateOrProvidence { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public int Altitude { get; set; }

        public double Timezone { get; set; }

        public string TimezoneName { get; set; }

        public string DST { get; set; }
    }

    public class AirportList
    {
        public List<Airport> Airports { get; set; }

        public Bounds Bounds { get; set; }
    }
}