using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // First and last names for profile
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Navigation relationships
        public ICollection<Story> Stories { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<EventAttendee> EventAttendances { get; set; }
        public ICollection<CommunityMember> CommunityMemberships { get; set; }
        public ICollection<MasjidVisit> Visits { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Blog> BlogsCreated { get; set; }
    }
}
