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
        public async Task<ActionResult<List<string>>> GetAllTags()
        {
            var tags = await _tagRepo.FindAsync(t => true);
            return Ok(tags.Select(t => t.Name).ToList());
        }
    }
}
