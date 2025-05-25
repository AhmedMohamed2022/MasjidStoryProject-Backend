using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Localized content for masjids
    public class MasjidContent
    {
        public int Id { get; set; }
        public int MasjidId { get; set; }
        public virtual Masjid Masjid { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
