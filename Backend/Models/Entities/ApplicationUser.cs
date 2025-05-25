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
        public virtual ICollection<Story> Stories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Event> EventsCreated { get; set; }
        public virtual ICollection<EventAttendee> EventAttendances { get; set; }
        public virtual ICollection<CommunityMember> CommunityMemberships { get; set; }
        public virtual ICollection<MasjidVisit> Visits { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Blog> BlogsCreated { get; set; }
    }
}
