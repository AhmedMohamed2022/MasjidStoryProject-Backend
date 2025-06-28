using Models.Entities;
using Repositories.Interfaces;
using ViewModels;

namespace Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IBaseRepository<ApplicationUser> _baseRepo;

        public UserRepository(IBaseRepository<ApplicationUser> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _baseRepo.GetByIdAsync(id);
        }

        public async Task<UserProfileViewModel?> GetProfileAsync(string id)
        {
            var user = await _baseRepo.GetByIdAsync(id);
            return user?.ToViewModel();
        }

        public async Task<bool> UpdateProfileAsync(string id, UserProfileUpdateViewModel model)
        {
            var user = await _baseRepo.GetByIdAsync(id);
            if (user == null) return false;

            model.UpdateEntity(user);
            _baseRepo.Update(user);
            await _baseRepo.SaveChangesAsync();

            return true;
        }
    }
}
