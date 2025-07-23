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

            builder.Property(e => e.EventDate)
                .IsRequired();

            builder.HasMany(e => e.Contents)
                .WithOne(c => c.Event)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Masjid)
                .WithMany(m => m.Events)
                .HasForeignKey(e => e.MasjidId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.CreatedBy)
                .WithMany(u => u.EventsCreated)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
