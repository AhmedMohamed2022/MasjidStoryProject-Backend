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
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

               builder.Property(b => b.Content)
                .IsRequired();

            builder.Property(b => b.DatePublished)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.CreatedBy)
                .WithMany(u=> u.BlogsCreated)
                .HasForeignKey(b => b.CreatedById);
        }
    }
}
