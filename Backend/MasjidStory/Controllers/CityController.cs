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
        public async Task<ActionResult<ApiResponse<List<CityViewModel>>>> GetAllCities()
        {
            var cities = await _cityRepo.GetAllAsync(c => c.Country);
            var viewModels = cities.Select(c => new CityViewModel
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId,
                CountryName = c.Country?.Name ?? ""
            }).ToList();

            return Ok(ApiResponse<List<CityViewModel>>.Ok(viewModels));
        }

        // GET: api/city/bycountry/{countryId}
        [HttpGet("bycountry/{countryId}")]
        public async Task<ActionResult<ApiResponse<List<CityViewModel>>>> GetCitiesByCountry(int countryId)
        {
            var cities = await _cityRepo.FindAsync(c => c.CountryId == countryId, c => c.Country);
            var viewModels = cities.Select(c => new CityViewModel
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId,
                CountryName = c.Country?.Name ?? ""
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