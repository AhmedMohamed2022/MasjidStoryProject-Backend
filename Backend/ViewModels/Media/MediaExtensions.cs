using Models.Entities;
using ViewModels;

namespace ViewModels
{
    public static class MediaExtensions
    {
        public static MediaViewModel ToViewModel(this Media media)
        {
            return new MediaViewModel
            {
                Id = media.Id,
                FileUrl = media.FileUrl,
                //FileName = media.FileName,
                //FileSize = media.FileSize,
                ContentType = media.MediaType,
                MasjidId = media.MasjidId,
                StoryId = media.StoryId,
                UploadDate = media.DateUploaded
            };
        }
    }
}