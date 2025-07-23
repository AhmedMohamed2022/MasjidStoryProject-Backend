using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Models.Entities;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly IBaseRepository<Tag> _tagRepo;

        public TagController(IBaseRepository<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }

        // GET: api/tag/all
        [HttpGet("all")]
        public async Task<ActionResult<List<object>>> GetAllTags([FromQuery] string languageCode = "en")
        {
            var tags = await _tagRepo.FindAsync(t => true);
            var tagObjects = tags.Select(t =>
            {
                var content = t.Contents?.FirstOrDefault(tc => tc.Language != null && tc.Language.Code == languageCode)
                    ?? t.Contents?.FirstOrDefault(tc => tc.Language != null && tc.Language.Code == "en")
                    ?? t.Contents?.FirstOrDefault();
                return new {
                    id = t.Id,
                    localizedName = content?.Name ?? ""
                };
            })
            .Where(t => !string.IsNullOrWhiteSpace(t.localizedName))
            .ToList();
            return Ok(tagObjects);
        }
    }
}
