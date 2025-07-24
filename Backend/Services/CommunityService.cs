using Models.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class CommunityService
    {
        private readonly IBaseRepository<Community> _repo;
        private readonly IBaseRepository<CommunityMember> _memberRepo;

        public CommunityService(IBaseRepository<Community> repo, IBaseRepository<CommunityMember> memberRepo)
        {
            _repo = repo;
            _memberRepo = memberRepo;
        }

        public async Task<List<CommunityViewModel>> GetMasjidCommunitiesAsync(int masjidId, string? userId, string languageCode = "en")
        {
            var communities = await _repo.FindAsync(
                c => c.MasjidId == masjidId,
                c => c.Contents, c => c.Masjid, c => c.CreatedBy, c => c.CommunityMembers, c => c.Contents.Select(cc => cc.Language)
            );
            return communities.Select(c => c.ToViewModel(userId, languageCode)).ToList();
        }

        public async Task<CommunityViewModel?> GetByIdAsync(int id, string? userId, string languageCode = "en")
        {
            var community = await _repo.GetFirstOrDefaultAsync(
                c => c.Id == id,
                c => c.Contents, c => c.Masjid, c => c.CreatedBy, c => c.CommunityMembers, c => c.Contents.Select(cc => cc.Language)
            );
            return community?.ToViewModel(userId, languageCode);
        }

        public async Task CreateAsync(CommunityCreateViewModel model, string userId)
        {
            var entity = model.ToEntity(userId);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task<bool> JoinCommunityAsync(int communityId, string userId)
        {
            var exists = await _memberRepo.AnyAsync(m => m.CommunityId == communityId && m.UserId == userId);
            if (exists) return false;

            await _memberRepo.AddAsync(new CommunityMember
            {
                CommunityId = communityId,
                UserId = userId,
                JoinedDate = DateTime.UtcNow
            });

            await _memberRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveCommunityAsync(int communityId, string userId)
        {
            var membership = (await _memberRepo.FindAsync(m => m.CommunityId == communityId && m.UserId == userId)).FirstOrDefault();
            if (membership == null) return false;

            _memberRepo.Delete(membership);
            await _memberRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<CommunityViewModel>> GetUserCommunitiesAsync(string userId, string languageCode = "en")
        {
            var memberships = await _memberRepo.FindAsync(m => m.UserId == userId, m => m.Community, m => m.Community.Contents, m => m.Community.Contents.Select(cc => cc.Language));
            var communities = memberships.Select(m => m.Community).Distinct().ToList();
            return communities.Select(c => c.ToViewModel(userId, languageCode)).ToList();
        }
        public async Task<bool> UpdateCommunityAsync(int id, CommunityCreateViewModel model, string userId)
        {
            var community = await _repo.GetFirstOrDefaultAsync(c => c.Id == id, c => c.Contents, c => c.Contents.Select(cc => cc.Language));
            if (community == null || community.CreatedById != userId) return false;
            community.MasjidId = model.MasjidId;
            // Replace all translations with the provided ones
            community.Contents = model.Contents?.Select(cc => new CommunityContent {
                LanguageId = cc.LanguageId,
                Title = cc.Title,
                Content = cc.Content
            }).ToList() ?? new List<CommunityContent>();
            _repo.Update(community);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCommunityAsync(int id, string userId)
        {
            var community = await _repo.GetByIdAsync(id);
            if (community == null || community.CreatedById != userId) return false;

            // First, delete all community members
            var members = await _memberRepo.FindAsync(m => m.CommunityId == id);
            foreach (var member in members)
            {
                _memberRepo.Delete(member);
            }

            // Then delete the community
            _repo.Delete(community);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<CommunityViewModel>> GetAllCommunitiesAsync(string? userId, string languageCode = "en")
        {
            var communities = await _repo.FindAsync(
                c => true, // Get all communities
                c => c.Contents, c => c.Masjid, c => c.CreatedBy, c => c.CommunityMembers, c => c.Contents.Select(cc => cc.Language)
            );
            return communities.Select(c => c.ToViewModel(userId, languageCode)).ToList();
        }
    }

}
