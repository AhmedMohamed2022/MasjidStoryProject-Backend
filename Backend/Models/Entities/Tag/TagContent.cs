using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // TagContent for multilingual tag names
    public class TagContent
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Name { get; set; }
    }
} 