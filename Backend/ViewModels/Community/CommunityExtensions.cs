using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using ViewModels;

namespace ViewModels
{
    public static class CommunityExtensions
    {
        public static CommunityViewModel ToViewModel(this Community c, string? userId = null, string languageCode = "en")
        {
            var content = c.Contents?.FirstOrDefault(x => x.Language != null && x.Language.Code == languageCode)
                ?? c.Contents?.FirstOrDefault();
            return new CommunityViewModel
            {
                Id = c.Id,
                LocalizedTitle = content?.Title ?? string.Empty,
                LocalizedContent = content?.Content ?? string.Empty,
                MasjidId = c.MasjidId,
                MasjidName = c.Masjid?.Contents?.FirstOrDefault(mc => mc.Language != null && mc.Language.Code == languageCode)?.Name ?? c.Masjid?.Contents?.FirstOrDefault()?.Name,
                CreatedByName = $"{c.CreatedBy?.FirstName} {c.CreatedBy?.LastName}",
                DateCreated = c.DateCreated,
                IsUserMember = userId != null && c.CommunityMembers?.Any(m => m.UserId == userId) == true,
                MemberCount = c.CommunityMembers?.Count ?? 0,
                Contents = c.Contents?.Select(cc => new CommunityContentViewModel {
                    LanguageId = cc.LanguageId,
                    Title = cc.Title,
                    Content = cc.Content
                }).ToList() ?? new List<CommunityContentViewModel>()
            };
        }

        public static Community ToEntity(this CommunityCreateViewModel vm, string userId)
        {
            return new Community
            {
                MasjidId = vm.MasjidId,
                CreatedById = userId,
                DateCreated = DateTime.UtcNow,
                Contents = vm.Contents?.Select(cc => new CommunityContent {
                    LanguageId = cc.LanguageId,
                    Title = cc.Title,
                    Content = cc.Content
                }).ToList() ?? new List<CommunityContent>()
            };
        }
    }

}
