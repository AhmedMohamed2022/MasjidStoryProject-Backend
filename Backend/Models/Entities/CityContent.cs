using System;
using System.Collections.Generic;

namespace Models.Entities
{
    // Localized content for cities
    public class CityContent
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Name { get; set; }
    }
} 