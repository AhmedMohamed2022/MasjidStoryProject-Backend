using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Repositories;
using Repositories.Implementations;
using Repositories.Interfaces;
using ViewModels;
namespace Services
{
    public class MasjidService
    {
        private readonly IMasjidRepository _repository;
        private readonly IMasjidVisitRepository _masjidVisitRepository;
        public MasjidService(IMasjidRepository repository,IMasjidVisitRepository masjidVisitRepository)
        {
            _repository = repository;
            _masjidVisitRepository = masjidVisitRepository;
        }

        public async Task<List<MasjidViewModel>> GetAllMasjidsAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<List<MasjidViewModel>> GetMasjidsPagedAsync(string? search, int page, int size)
        {
            return await _repository.GetFilteredAsync(search, page, size);
        }

        public async Task<MasjidViewModel> GetMasjidByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddMasjidAsync(MasjidCreateViewModel model)
        {
            await _repository.AddAsync(model);
        }

        public async Task<bool> UpdateMasjidAsync(MasjidEditViewModel model)
        {
            return await _repository.UpdateAsync(model);
        }

        public async Task<bool> DeleteMasjidAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<MasjidDetailsViewModel?> GetMasjidDetailsAsync(int id, string? lang = null)
        {
            return await _repository.GetMasjidDetailsAsync(id, lang);
        }
        public async Task<bool> RegisterVisitAsync(int masjidId, string userId)
        {
            var masjid = await _repository.GetByIdAsync(masjidId);
            if (masjid == null) return false;

            var visit = new MasjidVisit
            {
                MasjidId = masjidId,
                UserId = userId,
                VisitDate = DateTime.UtcNow
            };

            await _masjidVisitRepository.AddAsync(visit.ToCreateViewModel());
            return true;
        }
        public async Task<List<MasjidViewModel>> GetFeaturedMasjidsAsync()
        {
            var masjids = await _repository.GetAllAsync(
                m => m.Visits, m => m.Stories, m => m.Country, m => m.City
            );

            var featured = masjids
                .OrderByDescending(m => m.Visits?.Count ?? 0)
                .ThenByDescending(m => m.Stories?.Count ?? 0)
                .Take(6)
                .Select(m => m.ToViewModel())
                .ToList();

            return featured;
        }


    }

}
