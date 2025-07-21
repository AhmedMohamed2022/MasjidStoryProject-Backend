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
        public static CommunityViewModel ToViewModel(this Community c, string? userId = null)
        {
            return new CommunityViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                MasjidId = c.MasjidId,
                LanguageCode = c.Language?.Code ?? "en",
                CreatedByName = $"{c.CreatedBy?.FirstName} {c.CreatedBy?.LastName}",
                DateCreated = c.DateCreated,
                IsUserMember = userId != null && c.CommunityMembers?.Any(m => m.UserId == userId) == true,
                MemberCount = c.CommunityMembers?.Count ?? 0
            };
        }

        public static Community ToEntity(this CommunityCreateViewModel vm, string userId)
        {
            return new Community
            {
                Title = vm.Title,
                Content = vm.Content,
                MasjidId = vm.MasjidId,
                LanguageId = vm.LanguageId,
                CreatedById = userId,
                DateCreated = DateTime.UtcNow
            };
        }
    }

}
