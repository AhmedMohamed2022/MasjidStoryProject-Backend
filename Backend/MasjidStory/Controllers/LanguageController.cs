using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly LanguageService _languageService;

        public LanguageController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        // GET: api/language/all
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<LanguageViewModel>>>> GetAllLanguages()
        {
            var languages = await _languageService.GetAllLanguagesAsync();
            var viewModels = languages.Select(l => new LanguageViewModel
            {
                Id = l.Id,
                Name = l.Name,
                Code = l.Code
            }).ToList();

            return Ok(ApiResponse<List<LanguageViewModel>>.Ok(viewModels));
        }

        // GET: api/language/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LanguageViewModel>>> GetLanguageById(int id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);
            if (language == null)
                return NotFound();

            var viewModel = new LanguageViewModel
            {
                Id = language.Id,
                Name = language.Name,
                Code = language.Code
            };

            return Ok(ApiResponse<LanguageViewModel>.Ok(viewModel));
        }
    }
}