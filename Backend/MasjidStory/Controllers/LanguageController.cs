using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly LanguageService _service;

        public LanguageController(LanguageService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<LanguageViewModel>>> GetAll()
        {
            var languages = await _service.GetAllLanguagesAsync();
            return Ok(ApiResponse<List<LanguageViewModel>>.Ok(languages));
        }
    }

}
