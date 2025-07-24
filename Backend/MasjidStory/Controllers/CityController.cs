using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Models.Entities;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly IBaseRepository<City> _cityRepo;

        public CityController(IBaseRepository<City> cityRepo)
        {
            _cityRepo = cityRepo;
        }

        // GET: api/city/all
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<CityViewModel>>>> GetAllCities([FromQuery] string languageCode = "en")
        {
            var langId = languageCode.ToLower() == "ar" ? 2 : 1;
            var cities = await _cityRepo.GetAllAsync(c => c.Country);
            var viewModels = cities.Select(c => new CityViewModel
            {
                Id = c.Id,
                Name = c.Contents?.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    ?? c.Contents?.FirstOrDefault(x => x.LanguageId == 1)?.Name
                    ?? c.Contents?.FirstOrDefault()?.Name
                    ?? string.Empty,
                CountryId = c.CountryId,
                CountryName = c.Country?.Contents?.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    ?? c.Country?.Contents?.FirstOrDefault(x => x.LanguageId == 1)?.Name
                    ?? c.Country?.Contents?.FirstOrDefault()?.Name
                    ?? string.Empty
            }).ToList();

            return Ok(ApiResponse<List<CityViewModel>>.Ok(viewModels));
        }

        // GET: api/city/bycountry/{countryId}
        [HttpGet("bycountry/{countryId}")]
        public async Task<ActionResult<ApiResponse<List<CityViewModel>>>> GetCitiesByCountry(int countryId, [FromQuery] string languageCode = "en")
        {
            var langId = languageCode.ToLower() == "ar" ? 2 : 1;
            var cities = await _cityRepo.FindAsync(c => c.CountryId == countryId, c => c.Country);
            var viewModels = cities.Select(c => new CityViewModel
            {
                Id = c.Id,
                Name = c.Contents?.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    ?? c.Contents?.FirstOrDefault(x => x.LanguageId == 1)?.Name
                    ?? c.Contents?.FirstOrDefault()?.Name
                    ?? string.Empty,
                CountryId = c.CountryId,
                CountryName = c.Country?.Contents?.FirstOrDefault(x => x.LanguageId == langId)?.Name
                    ?? c.Country?.Contents?.FirstOrDefault(x => x.LanguageId == 1)?.Name
                    ?? c.Country?.Contents?.FirstOrDefault()?.Name
                    ?? string.Empty
            }).ToList();

            return Ok(ApiResponse<List<CityViewModel>>.Ok(viewModels));
        }
    }

    public class CityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}