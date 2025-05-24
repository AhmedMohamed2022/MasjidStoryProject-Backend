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
    public class MasjidVisitConfiguration : IEntityTypeConfiguration<MasjidVisit>
    {
        public void Configure(EntityTypeBuilder<MasjidVisit> builder)
        {
            builder.ToTable("MasjidVisits");

            builder.HasKey(mv => mv.Id);

            builder.Property(mv => mv.VisitDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(mv => mv.User)
                .WithMany()
                .HasForeignKey(mv => mv.UserId);

            builder.HasOne(mv => mv.Masjid)
                .WithMany(m => m.Visits)
                .HasForeignKey(mv => mv.MasjidId);
        }
    }
}
