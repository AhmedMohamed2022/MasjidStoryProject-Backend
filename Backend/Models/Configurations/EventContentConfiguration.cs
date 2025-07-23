using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class EventContentConfiguration : IEntityTypeConfiguration<EventContent>
    {
        public void Configure(EntityTypeBuilder<EventContent> builder)
        {
            builder.ToTable("EventContents");
            builder.HasKey(ec => ec.Id);
            builder.Property(ec => ec.Title).IsRequired().HasMaxLength(200);
            builder.Property(ec => ec.Description).IsRequired();
            builder.HasOne(ec => ec.Event)
                .WithMany(e => e.Contents)
                .HasForeignKey(ec => ec.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ec => ec.Language)
                .WithMany()
                .HasForeignKey(ec => ec.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

