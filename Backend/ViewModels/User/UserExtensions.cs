using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class UserExtensions
    {
        public static UserProfileViewModel ToViewModel(this ApplicationUser user)
        {
            return new UserProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = $"{user.FirstName} {user.LastName}",
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }

        public static void UpdateEntity(this UserProfileUpdateViewModel model, ApplicationUser user)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.ProfilePictureUrl = model.ProfilePictureUrl;
        }
    }
}
