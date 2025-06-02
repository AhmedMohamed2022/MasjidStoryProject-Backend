using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace ViewModels
{
    public class MasjidDetailsViewModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string ArchStyle { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? YearOfEstablishment { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        // Localized content
        public string LocalizedName { get; set; }
        public string LocalizedDescription { get; set; }

        // Media files
        public List<string> MediaUrls { get; set; }

        // Approved Stories
        public List<StorySummaryViewModel> Stories { get; set; }

        // Stats
        public int TotalVisits { get; set; }
        public int UpcomingEventCount { get; set; }
    }


}
