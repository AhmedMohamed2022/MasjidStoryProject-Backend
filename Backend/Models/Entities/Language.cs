using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Language for localization support
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }   // Arabic, English, etc.
        public string Code { get; set; }   // ar, en, etc.
        public virtual ICollection<MasjidContent> MasjidContents { get; set; }
        public virtual ICollection<Story> Stories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Community> Communities { get; set; }
    }
}
