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
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Masjid> Masjids { get; set; }
    }
}
