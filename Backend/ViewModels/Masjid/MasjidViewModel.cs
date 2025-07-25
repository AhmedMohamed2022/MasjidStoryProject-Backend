using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModels/Masjid/MasjidViewModel.cs
    public class MasjidViewModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string ArchStyle { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public int? YearOfEstablishment { get; set; }
        public DateTime DateOfRecord { get; set; }
        // All available language contents
        public List<MasjidContentViewModel> Contents { get; set; } = new();
        public string LocalizedName { get; set; }
    }

}
