using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Models.Entities;

namespace ViewModels
{
    // ViewModels/Masjid/MasjidExtensions.cs
    public static class MasjidExtensions
    {
        public static MasjidViewModel ToViewModel(this Masjid entity)
        {
            return new MasjidViewModel
            {
                Id = entity.Id,
                ShortName = entity.ShortName,
                Address = entity.Address,
                ArchStyle = entity.ArchStyle,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                CountryName = entity.Country?.Name,
                CityName = entity.City?.Name,
                YearOfEstablishment = entity.YearOfEstablishment,
                DateOfRecord = entity.DateOfRecord
            };
        }

        public static Masjid ToEntity(this MasjidCreateViewModel vm)
        {
            return new Masjid
            {
                ShortName = vm.ShortName,
                Address = vm.Address,
                ArchStyle = vm.ArchStyle,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                CountryId = vm.CountryId,
                CityId = vm.CityId,
                YearOfEstablishment = vm.YearOfEstablishment,
                DateOfRecord = DateTime.UtcNow
            };
        }

        public static void UpdateEntity(this MasjidEditViewModel vm, Masjid entity)
        {
            entity.ShortName = vm.ShortName;
            entity.Address = vm.Address;
            entity.ArchStyle = vm.ArchStyle;
            entity.Latitude = vm.Latitude;
            entity.Longitude = vm.Longitude;
            entity.CountryId = vm.CountryId;
            entity.CityId = vm.CityId;
            entity.YearOfEstablishment = vm.YearOfEstablishment;
        }
    }

}
