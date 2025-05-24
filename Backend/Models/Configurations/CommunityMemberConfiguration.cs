using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Models.Configurations
{
    public class CommunityMemberConfiguration : IEntityTypeConfiguration<CommunityMember>
    {
        public void Configure(EntityTypeBuilder<CommunityMember> builder)
        {
            builder.ToTable("CommunityMembers");

            builder.HasKey(cm => cm.Id); // Use new surrogate key

            builder.HasIndex(cm => new { cm.CommunityId, cm.UserId }).IsUnique(); // Enforce uniqueness
            builder.HasOne(cm => cm.User)
                .WithMany(u=> u.CommunityMemberships)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(cm => cm.Community)
                .WithMany(c => c.CommunityMembers)
                .HasForeignKey(cm => cm.CommunityId)
                .OnDelete(DeleteBehavior.NoAction);
            ;
        }
    }
}
