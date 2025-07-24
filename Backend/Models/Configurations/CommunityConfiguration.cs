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
    public class CommunityConfiguration : IEntityTypeConfiguration<Community>
    {
        public void Configure(EntityTypeBuilder<Community> builder)
        {
            builder.ToTable("Communities");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Masjid)
                .WithMany(m => m.Communities)
                .HasForeignKey(c => c.MasjidId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
