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
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .IsRequired();

            builder.Property(e => e.EventDate)
                .IsRequired();

            builder.HasOne(e => e.Masjid)
                .WithMany(m => m.Events)
                .HasForeignKey(e => e.MasjidId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.CreatedBy)
                .WithMany(ep => ep.EventsCreated)
                .HasForeignKey(ep => ep.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
