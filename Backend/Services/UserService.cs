using Repositories.Interfaces;
using ViewModels;

namespace Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly MediaService _mediaService;

        public UserService(IUserRepository repo,MediaService mediaService)
        {
            _repo = repo;
            _mediaService = mediaService;

        }

        public async Task<UserProfileViewModel?> GetProfileAsync(string id)
        {
            return await _repo.GetProfileAsync(id);
        }
        public async Task<bool> UpdateProfileAsync(string id, UserProfileUpdateViewModel model)
        {
            string? uploadedUrl = null;
            if (model.ProfilePicture != null)
            {
                uploadedUrl = await _mediaService.UploadUserProfilePictureAsync(model.ProfilePicture);
                model.ProfilePictureUrl = uploadedUrl; // Set the new URL in the model
            }
            return await _repo.UpdateProfileAsync(id, model);
        }
    }
}
