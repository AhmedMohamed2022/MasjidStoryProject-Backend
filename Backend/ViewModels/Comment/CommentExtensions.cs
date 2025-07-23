using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace ViewModels
{
    public static class CommentExtensions
    {
        // Convert entity to view model
        public static CommentViewModel ToViewModel(this Comment entity)
        {
            return new CommentViewModel
            {
                Id = entity.Id,
                ContentId = entity.ContentId,
                ContentType = entity.ContentType,
                StoryId = entity.StoryId ?? 0, // Handle nullable StoryId
                StoryTitle = entity.Story?.Contents?.FirstOrDefault()?.Title,
                UserId = entity.UserId,
                UserName = entity.Author?.UserName,
                Content = entity.Content,
                DatePosted = entity.DatePosted,
                IsActive = entity.IsActive
            };
        }

        // Convert create model to entity
        public static Comment ToEntity(this CommentCreateViewModel model, string userId)
        {
            return new Comment
            {
                ContentId = model.ContentId,
                ContentType = model.ContentType,
                StoryId = model.ContentType == "Story" ? model.ContentId : null, // Set StoryId only for stories
                Content = model.Content,
                UserId = userId,
                DatePosted = DateTime.UtcNow,
                IsActive = true
            };
        }

        // Update existing entity from edit model
        public static void UpdateEntity(this CommentEditViewModel model, Comment entity)
        {
            entity.Content = model.Content;
        }
    }
}
