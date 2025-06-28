using Models.Entities;
using ViewModels;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<UserProfileViewModel?> GetProfileAsync(string id);
        Task<bool> UpdateProfileAsync(string id, UserProfileUpdateViewModel model);
    }
}
