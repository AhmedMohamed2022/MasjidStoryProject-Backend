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
        public string Address { get; set; }
        public string ArchStyle { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public int? YearOfEstablishment { get; set; }
        public DateTime DateOfRecord { get; set; }
    }

}
