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
        public static MasjidViewModel ToViewModel(this Masjid masjid)
        {
            return new MasjidViewModel
            {
                Id = masjid.Id,
                //ShortName = masjid.ShortName,
                Address = masjid.Address,
                ArchStyle = masjid.ArchStyle,
                Latitude = masjid.Latitude,
                Longitude = masjid.Longitude,
                CountryId = masjid.CountryId,
                CityId = masjid.CityId,
                YearOfEstablishment = masjid.YearOfEstablishment,
                CountryName = masjid.Country?.Name,
                CityName = masjid.City?.Name,
                Contents = masjid.Contents?.Select(c => new MasjidContentViewModel {
                    LanguageId = c.LanguageId,
                    Name = c.Name,
                    Description = c.Description
                }).ToList() ?? new List<MasjidContentViewModel>()
            };
        }

        public static Masjid ToEntity(this MasjidCreateViewModel model)
        {
            var entity = new Masjid
            {
                Address = model.Address,
                ArchStyle = model.ArchStyle,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                CountryId = model.CountryId,
                CityId = model.CityId,
                YearOfEstablishment = model.YearOfEstablishment,
                Contents = model.Contents?.Select(c => new MasjidContent {
                    LanguageId = c.LanguageId,
                    Name = c.Name,
                    Description = c.Description
                }).ToList() ?? new List<MasjidContent>()
            };
            return entity;
        }

        public static void UpdateEntity(this MasjidEditViewModel model, Masjid entity)
        {
            entity.Address = model.Address;
            entity.ArchStyle = model.ArchStyle;
            entity.Latitude = model.Latitude;
            entity.Longitude = model.Longitude;
            entity.CountryId = model.CountryId;
            entity.CityId = model.CityId;
            entity.YearOfEstablishment = model.YearOfEstablishment;
            // Replace all translations with the provided ones
            entity.Contents = model.Contents?.Select(c => new MasjidContent {
                LanguageId = c.LanguageId,
                Name = c.Name,
                Description = c.Description
            }).ToList() ?? new List<MasjidContent>();
        }
        public static MasjidDetailsViewModel ToDetailsViewModel(this Masjid masjid, string? languageCode = null)
        {
            // Debug log: languageCode and available codes
            System.Diagnostics.Debug.WriteLine($"Requested languageCode: {languageCode}");
            foreach (var c in masjid.Contents)
            {
                System.Diagnostics.Debug.WriteLine($"Content Language: {c.Language?.Code}, Name: {c.Name}");
            }
            // Use the language code directly
            var content = masjid.Contents.FirstOrDefault(c => c.Language.Code == languageCode)
                          ?? masjid.Contents.FirstOrDefault();

            return new MasjidDetailsViewModel
            {
                Id = masjid.Id,
                //ShortName = masjid.ShortName,
                Address = masjid.Address,
                ArchStyle = masjid.ArchStyle,
                Latitude = masjid.Latitude,
                Longitude = masjid.Longitude,
                YearOfEstablishment = masjid.YearOfEstablishment,
                CountryName = masjid.Country?.Name,
                CityName = masjid.City?.Name,
                LocalizedName = content?.Name ?? masjid.Contents.FirstOrDefault()?.Name ?? "",
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