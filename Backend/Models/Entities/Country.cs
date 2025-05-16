using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Country entity for geographic grouping
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }  // e.g. EG, SA
        public ICollection<City> Cities { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Masjid> Masjids { get; set; }
    }
}
