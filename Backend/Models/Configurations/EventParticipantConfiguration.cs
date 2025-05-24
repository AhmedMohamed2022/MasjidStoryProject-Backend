using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace Models.Configurations
{
    public class EventAttendeeConfiguration : IEntityTypeConfiguration<EventAttendee>
    {
        public void Configure(EntityTypeBuilder<EventAttendee> builder)
        {
            builder.ToTable("EventParticipants");

            builder.HasKey(ep => new { ep.EventId, ep.UserId });

            builder.HasOne(ep => ep.User)
                .WithMany(u=>u.EventAttendances)
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ep => ep.Event)
                .WithMany(e => e.EventAttendees)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
