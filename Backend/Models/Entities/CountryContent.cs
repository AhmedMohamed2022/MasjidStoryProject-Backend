using System;
using System.Collections.Generic;

namespace Models.Entities
{
    // Localized content for countries
    public class CountryContent
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Name { get; set; }
    }
} 