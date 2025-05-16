using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Core mosque entity with location and metadata
    public class Masjid
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Address { get; set; }
        public string ArchStyle { get; set; }
        public DateTime DateOfRecord { get; set; } = DateTime.UtcNow;
        public int? YearOfEstablishment { get; set; }

        // Multilingual content and media
        public ICollection<MasjidContent> Contents { get; set; }
        public ICollection<Media> MediaItems { get; set; }
        public ICollection<Story> Stories { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Community> Communities { get; set; }
    }
}
