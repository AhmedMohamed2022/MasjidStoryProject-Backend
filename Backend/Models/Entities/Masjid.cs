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
        public virtual Country Country { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Address { get; set; }
        public string ArchStyle { get; set; }
        public DateTime DateOfRecord { get; set; } = DateTime.UtcNow;
        public int? YearOfEstablishment { get; set; }

        // Multilingual content and media
        public virtual ICollection<MasjidContent> Contents { get; set; }
        public virtual ICollection<MasjidVisit> Visits { get; set; }

        public virtual ICollection<Media> MediaItems { get; set; }
        public virtual ICollection<Story> Stories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public  virtual ICollection<Community> Communities { get; set; }
    }
}
