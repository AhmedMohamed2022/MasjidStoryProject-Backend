using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // City entity linked to country
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public virtual  Country Country { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Masjid> Masjids { get; set; }
        // Add translation support
        public virtual ICollection<CityContent> Contents { get; set; }
    }
}
