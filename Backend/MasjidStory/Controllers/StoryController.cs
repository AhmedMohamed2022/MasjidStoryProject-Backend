using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly StoryService _service;
        private readonly MediaService _mediaService;
        private readonly ContentModerationService _contentModerationService;

        public StoryController(StoryService service, MediaService mediaService, ContentModerationService contentModerationService)
        {
            _service = service;
            _mediaService = mediaService;
            _contentModerationService = contentModerationService;
        }

        // GET: api/story/all
        [HttpGet("all")]
        public async Task<ActionResult<List<StoryViewModel>>> GetAll([FromQuery] string languageCode = "en")
        {
            var stories = await _service.GetAllStoriesAsync(languageCode);
            return Ok(stories);
        }

        // GET: api/story/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginatedResponse<StoryViewModel>>> GetPaginated([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string languageCode = "en")
        {
            var result = await _service.GetStoriesPaginatedAsync(page, size, languageCode);
            return Ok(result);
        }

        // GET: api/story/my-stories
        [HttpGet("my-stories")]
        [Authorize]
        public async Task<ActionResult<List<StoryViewModel>>> GetMyStories([FromQuery] string languageCode = "en")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var stories = await _service.GetStoriesByUserIdAsync(userId, languageCode);
            return Ok(stories);
        }

        // GET: api/story/details/{id}
        [HttpGet("details/{id}")]
        public async Task<ActionResult<StoryViewModel>> GetById(int id, [FromQuery] string languageCode = "en")
        {
            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var story = await _service.GetStoryByIdAsync(id, userId, languageCode);
            if (story == null)
                return NotFound();

            return Ok(story);
        }

        // POST: api/story/add
        [Authorize]
        [HttpPost("add")]
        [RequestSizeLimit(10 * 1024 * 1024)] // Max 10 MB total for uploaded images
        public async Task<IActionResult> AddStory([FromForm] StoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();

                var storyId= await _service.AddStoryAsync(model, userId);

                // Handle media uploads if any
                if (model.StoryImages != null && model.StoryImages.Any())
                {
                    foreach (var file in model.StoryImages)
                    {
                        var mediaModel = new MediaCreateViewModel
                        {
                            StoryId = storyId,
                            File = file
                        };
                        await _mediaService.UploadMediaAsync(mediaModel);
                    }
                }

                return Ok(ApiResponse<string>.Ok("Story submitted and pending approval."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Failed to submit a story: {ex.Message}"));
            }
        }

        // PUT: api/story/update/{id}
        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] StoryEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");
            try
            {
                // Get current story with media items
                var currentStory = await _service.GetStoryByIdAsync(id);
                if (currentStory == null)
                    return NotFound();

                // Analyze content changes to determine if re-approval is needed
                var contentAnalysis = _contentModerationService.AnalyzeContentChanges(model, currentStory);
                
                // Set the original content for tracking
                model.OriginalTitle = currentStory.Contents?.FirstOrDefault()?.Title;
                model.OriginalContent = currentStory.Contents?.FirstOrDefault()?.Content;
                model.RequiresReapproval = contentAnalysis.RequiresReapproval;
                model.ChangeReason = string.Join(", ", contentAnalysis.ChangeReason);

                // If significant changes detected, set story to pending
                if (contentAnalysis.RequiresReapproval)
                {
                    model.IsApproved = false;
                }

                // Handle media deletions based on KeepMediaIds
                if (currentStory.MediaItems != null && currentStory.MediaItems.Any())
                {
                    var currentMediaIds = currentStory.MediaItems.Select(m => m.Id).ToList();
                    var keepMediaIds = model.KeepMediaIds ?? new List<int>();
                    
                    // Find media items to delete (those not in keepMediaIds)
                    var mediaToDelete = currentMediaIds.Except(keepMediaIds).ToList();
                    
                    foreach (var mediaId in mediaToDelete)
                    {
                        await _mediaService.DeleteMediaAsync(mediaId);
                    }
                }

                // Handle specific media removals
                if (model.RemoveMediaIds != null && model.RemoveMediaIds.Any())
                {
                    foreach (var mediaId in model.RemoveMediaIds)
                    {
                        await _mediaService.DeleteMediaAsync(mediaId);
                    }
                }

                // Handle new media uploads
                if (model.NewStoryImages != null && model.NewStoryImages.Any())
                {
                    foreach (var file in model.NewStoryImages)
                    {
                        var mediaModel = new MediaCreateViewModel
                        {
                            StoryId = id,
                            File = file
                        };
                        await _mediaService.UploadMediaAsync(mediaModel);
                    }
                }

                var result = await _service.UpdateStoryAsync(model);
                if (!result)
                    return NotFound();

                // Return appropriate message based on content analysis
                if (contentAnalysis.RequiresReapproval)
                {
                    return Ok(ApiResponse<string>.Ok($"Story updated successfully. Due to significant changes ({string.Join(", ", contentAnalysis.ChangeReason)}), the story has been set to pending for admin approval."));
                }
                else
                {
                    return Ok(ApiResponse<string>.Ok("Story updated successfully."));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Failed to update story: {ex.Message}"));
            }
        }

        // DELETE: api/story/delete/{id}
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteStoryAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/story/pending
        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<StoryViewModel>>> GetPendingApproval([FromQuery] string languageCode = "en")
        {
            var stories = await _service.GetPendingStoriesAsync(languageCode);
            return Ok(stories);
        }

        // PUT: api/story/approve/{id}
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveStory(int id)
        {
            var result = await _service.ApproveStoryAsync(id);
            if (!result) return NotFound();

            return Ok(ApiResponse<string>.Ok("Story Approved."));
        }

        // GET: api/story/latest
        [HttpGet("latest")]
        public async Task<ActionResult<List<StoryViewModel>>> GetLatestStories([FromQuery] string languageCode = "en")
        {
            var stories = await _service.GetLatestStoriesAsync(languageCode);
            return Ok(ApiResponse<List<StoryViewModel>>.Ok(stories));
        }

        // GET: api/story/related/{storyId}
        [HttpGet("related/{storyId}")]
        public async Task<ActionResult<List<StoryViewModel>>> GetRelatedStories(int storyId, [FromQuery] string languageCode = "en")
        {
            var relatedStories = await _service.GetRelatedStoriesAsync(storyId, languageCode);
            return Ok(ApiResponse<List<StoryViewModel>>.Ok(relatedStories));
        }

        // DELETE: api/story/{storyId}/media/{mediaId}
        [HttpDelete("{storyId}/media/{mediaId}")]
        [Authorize]
        public async Task<IActionResult> DeleteStoryMedia(int storyId, int mediaId)
        {
            try
            {
                // Verify the story belongs to the current user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();

                var story = await _service.GetStoryByIdAsync(storyId, userId);
                if (story == null) return NotFound();

                // Verify the media belongs to this story
                var mediaExists = story.MediaItems?.Any(m => m.Id == mediaId) == true;
                if (!mediaExists) return NotFound();

                var result = await _mediaService.DeleteMediaAsync(mediaId);
                if (!result) return NotFound();

                return Ok(ApiResponse<string>.Ok("Media deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail($"Failed to delete media: {ex.Message}"));
            }
        }
    }
}
