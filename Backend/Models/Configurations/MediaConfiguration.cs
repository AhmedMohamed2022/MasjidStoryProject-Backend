////using Microsoft.EntityFrameworkCore.Metadata.Builders;
////using Microsoft.EntityFrameworkCore;
////using Models.Entities;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace Models.Configurations
////{
////    public class MediaConfiguration : IEntityTypeConfiguration<Media>
////    {
////        public void Configure(EntityTypeBuilder<Media> builder)
////        {
////            builder.ToTable("Media");
////            builder.HasKey(md => md.Id);
////            builder.Property(md => md.FileUrl)
////                   .IsRequired();
////            builder.Property(md => md.MediaType)
////                   .HasMaxLength(50)
////                   .HasDefaultValue("Image");
////            builder.Property(md => md.DateUploaded)
////                   .HasDefaultValueSql("GETUTCDATE()");
////            builder.HasOne(md => md.Masjid)
////                   .WithMany(m => m.MediaItems)
////                   .HasForeignKey(md => md.MasjidId)
////                   .OnDelete(DeleteBehavior.Restrict);
////            builder.HasOne(m => m.Story)
////                .WithMany(s => s.MediaItems)
////                .HasForeignKey(s => s.StoryId)
////                .OnDelete(DeleteBehavior.Restrict);
////        }
////    }
////}
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Models.Entities;

//namespace Models.Configurations
//{
//    public class MediaConfiguration : IEntityTypeConfiguration<Media>
//    {
//        public void Configure(EntityTypeBuilder<Media> builder)
//        {
//            builder.ToTable("Media");
//            builder.HasKey(m => m.Id);

//            builder.Property(m => m.FileUrl)
//                .IsRequired()
//                .HasMaxLength(500);

//            //builder.Property(m => m.FileName)
//            //    .HasMaxLength(255);

//            builder.Property(m => m.MediaType)
//                .HasMaxLength(100);

//            //builder.Property(m => m.FileSize)
//            //    .IsRequired();

//            //builder.Property(m => m.UploadDate)
//            //    .IsRequired();

//            // Foreign key relationships with cascade delete
//            builder.HasOne<Masjid>()
//                .WithMany(m => m.MediaItems)
//                .HasForeignKey(m => m.MasjidId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .IsRequired(false);

//            builder.HasOne<Story>()
//                .WithMany(s => s.MediaItems)
//                .HasForeignKey(m => m.StoryId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .IsRequired(false);

//            // Indexes for better performance
//            builder.HasIndex(m => m.MasjidId);
//            builder.HasIndex(m => m.StoryId);
//            builder.HasIndex(m => m.DateUploaded);
//        }
//    }
//}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("Media");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.FileName)
                .HasMaxLength(255);

            builder.Property(m => m.ContentType)
                .HasMaxLength(100);

            builder.Property(m => m.FileSize)
                .IsRequired();

            builder.Property(m => m.MediaType)
                .HasMaxLength(50)
                .HasDefaultValue("Image");

            builder.Property(m => m.DateUploaded)
                .HasDefaultValueSql("GETUTCDATE()");
            // Foreign key relationships with cascade delete
            builder.HasOne<Masjid>()
                .WithMany(m => m.MediaItems)
                .HasForeignKey(m => m.MasjidId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne<Story>()
                .WithMany(s => s.MediaItems)
                .HasForeignKey(m => m.StoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Indexes for better performance
            builder.HasIndex(m => m.MasjidId);
            builder.HasIndex(m => m.StoryId);
            builder.HasIndex(m => m.DateUploaded);
        }
    }
}