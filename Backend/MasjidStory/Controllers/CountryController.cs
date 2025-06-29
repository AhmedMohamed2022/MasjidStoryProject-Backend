using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Models.Entities;
using ViewModels;

namespace MasjidStory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IBaseRepository<Country> _countryRepo;

        public CountryController(IBaseRepository<Country> countryRepo)
        {
            _countryRepo = countryRepo;
        }

        // GET: api/country/all
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<CountryViewModel>>>> GetAllCountries()
        {
            var countries = await _countryRepo.FindAsync(c => true);
            var viewModels = countries.Select(c => new CountryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            }).ToList();

            return Ok(ApiResponse<List<CountryViewModel>>.Ok(viewModels));
        }
    }

    public class CountryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}