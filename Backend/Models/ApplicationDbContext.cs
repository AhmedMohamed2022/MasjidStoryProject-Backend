using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // DbSets
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Masjid> Masjids { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MasjidContent> MasjidContents { get; set; }
        public DbSet<Media> MediaItems { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<StoryTag> StoryTags { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MasjidVisit> MasjidVisits { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {



            base.OnModelCreating(builder);
        }
    }
}
