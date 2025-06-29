//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ViewModels;
//using Models.Entities;

//namespace ViewModels
//{
//    // ViewModels/Masjid/MasjidExtensions.cs
//    public static class MasjidExtensions
//    {
//        public static MasjidViewModel ToViewModel(this Masjid entity)
//        {
//            return new MasjidViewModel
//            {
//                Id = entity.Id,
//                ShortName = entity.ShortName,
//                Address = entity.Address,
//                ArchStyle = entity.ArchStyle,
//                Latitude = entity.Latitude,
//                Longitude = entity.Longitude,
//                CountryName = entity.Country?.Name,
//                CityName = entity.City?.Name,
//                YearOfEstablishment = entity.YearOfEstablishment,
//                DateOfRecord = entity.DateOfRecord
//            };
//        }

//        public static Masjid ToEntity(this MasjidCreateViewModel vm)
//        {
//            return new Masjid
//            {
//                ShortName = vm.ShortName,
//                Address = vm.Address,
//                ArchStyle = vm.ArchStyle,
//                Latitude = vm.Latitude,
//                Longitude = vm.Longitude,
//                CountryId = vm.CountryId,
//                CityId = vm.CityId,
//                YearOfEstablishment = vm.YearOfEstablishment,
//                DateOfRecord = DateTime.UtcNow
//            };
//        }

//        public static void UpdateEntity(this MasjidEditViewModel vm, Masjid entity)
//        {
//            entity.ShortName = vm.ShortName;
//            entity.Address = vm.Address;
//            entity.ArchStyle = vm.ArchStyle;
//            entity.Latitude = vm.Latitude;
//            entity.Longitude = vm.Longitude;
//            entity.CountryId = vm.CountryId;
//            entity.CityId = vm.CityId;
//            entity.YearOfEstablishment = vm.YearOfEstablishment;
//        }
//        public static MasjidDetailsViewModel ToDetailsViewModel(this Masjid masjid, string? languageCode = null)
//        {
//            var content = masjid.Contents.FirstOrDefault(c => c.Language.Code == languageCode)
//                          ?? masjid.Contents.FirstOrDefault();

//            return new MasjidDetailsViewModel
//            {
//                Id = masjid.Id,
//                ShortName = masjid.ShortName,
//                Address = masjid.Address,
//                ArchStyle = masjid.ArchStyle,
//                Latitude = masjid.Latitude,
//                Longitude = masjid.Longitude,
//                YearOfEstablishment = masjid.YearOfEstablishment,
//                CountryName = masjid.Country?.Name,
//                CityName = masjid.City?.Name,
//                LocalizedName = content?.Name ?? masjid.ShortName,
//                LocalizedDescription = content?.Description ?? "",
//                MediaUrls = masjid.MediaItems?.Select(m => m.FileUrl).ToList() ?? new(),
//                Stories = masjid.Stories
//                    .Where(s => s.IsApproved)
//                    .Select(s => new StorySummaryViewModel
//                    {
//                        Id = s.Id,
//                        Title = s.Title,
//                        AuthorName = $"{s.ApplicationUser.FirstName} {s.ApplicationUser.LastName}",
//                        DatePublished = s.DatePublished,
//                        LikeCount = s.Likes?.Count ?? 0,
//                        CommentCount = s.Comments?.Count ?? 0
//                    }).ToList(),
//                TotalVisits = masjid.Visits?.Count ?? 0,
//                UpcomingEventCount = masjid.Events?.Count(e => e.EventDate > DateTime.UtcNow) ?? 0
//            };
//        }

//    }

//}
using Models.Entities;
using ViewModels;

namespace ViewModels
{
    public static class MasjidExtensions
    {
        public static MasjidViewModel ToViewModel(this Masjid masjid)
        {
            return new MasjidViewModel
            {
                Id = masjid.Id,
                ShortName = masjid.ShortName,
                Address = masjid.Address,
                ArchStyle = masjid.ArchStyle,
                Latitude = masjid.Latitude,
                Longitude = masjid.Longitude,
                CountryId = masjid.CountryId,
                CityId = masjid.CityId,
                YearOfEstablishment = masjid.YearOfEstablishment,
                CountryName = masjid.Country?.Name,
                CityName = masjid.City?.Name
            };
        }

        public static Masjid ToEntity(this MasjidCreateViewModel model)
        {
            return new Masjid
            {
                ShortName = model.ShortName,
                Address = model.Address,
                ArchStyle = model.ArchStyle,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                CountryId = model.CountryId,
                CityId = model.CityId,
                YearOfEstablishment = model.YearOfEstablishment
            };
        }

        public static void UpdateEntity(this MasjidEditViewModel model, Masjid entity)
        {
            entity.ShortName = model.ShortName;
            entity.Address = model.Address;
            entity.ArchStyle = model.ArchStyle;
            entity.Latitude = model.Latitude;
            entity.Longitude = model.Longitude;
            entity.CountryId = model.CountryId;
            entity.CityId = model.CityId;
            entity.YearOfEstablishment = model.YearOfEstablishment;
        }
        public static MasjidDetailsViewModel ToDetailsViewModel(this Masjid masjid, string? languageCode = null)
        {
            var content = masjid.Contents.FirstOrDefault(c => c.Language.Code == languageCode)
                          ?? masjid.Contents.FirstOrDefault();

            return new MasjidDetailsViewModel
            {
                Id = masjid.Id,
                ShortName = masjid.ShortName,
                Address = masjid.Address,
                ArchStyle = masjid.ArchStyle,
                Latitude = masjid.Latitude,
                Longitude = masjid.Longitude,
                YearOfEstablishment = masjid.YearOfEstablishment,
                CountryName = masjid.Country?.Name,
                CityName = masjid.City?.Name,
                LocalizedName = content?.Name ?? masjid.ShortName,
                LocalizedDescription = content?.Description ?? "",
                MediaUrls = masjid.MediaItems?.Select(m => m.FileUrl).ToList() ?? new(),
                Stories = masjid.Stories
                    .Where(s => s.IsApproved)
                    .Select(s => new StorySummaryViewModel
                    {
                        Id = s.Id,
                        Title = s.Title,
                        AuthorName = $"{s.ApplicationUser.FirstName} {s.ApplicationUser.LastName}",
                        DatePublished = s.DatePublished,
                        LikeCount = s.Likes?.Count ?? 0,
                        CommentCount = s.Comments?.Count ?? 0
                    }).ToList(),
                TotalVisits = masjid.Visits?.Count ?? 0,
                UpcomingEventCount = masjid.Events?.Count(e => e.EventDate > DateTime.UtcNow) ?? 0
            };
        }
    }
}