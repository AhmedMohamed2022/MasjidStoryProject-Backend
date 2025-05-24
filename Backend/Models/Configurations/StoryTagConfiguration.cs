using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configurations
{
    public class StoryTagConfiguration : IEntityTypeConfiguration<StoryTag>
    {
        public void Configure(EntityTypeBuilder<StoryTag> builder)
        {
            builder.ToTable("StoryTags");

            builder.HasKey(st => new { st.StoryId, st.TagId });

            builder.HasOne(st => st.Story)
                .WithMany(s => s.StoryTags)
                .HasForeignKey(st => st.StoryId);

            builder.HasOne(st => st.Tag)
                .WithMany(t => t.StoryTags)
                .HasForeignKey(st => st.TagId);
        }
    }

}
