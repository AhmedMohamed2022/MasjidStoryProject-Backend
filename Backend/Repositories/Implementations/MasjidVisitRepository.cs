using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using ViewModels;
namespace Repositories
{

    public class MasjidVisitRepository : IMasjidVisitRepository
    {
        private readonly IBaseRepository<MasjidVisit> _baseRepo;

        public MasjidVisitRepository(IBaseRepository<MasjidVisit> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        // Fetch all Likes and map them to LikeViewModel
        public async Task<List<MasjidVisitViewModel>> GetAllAsync()
        {
            var likes = await _baseRepo.GetAllAsync();
            return likes.Select(l => l.ToViewModel()).ToList();
        }

        // Fetch Like by ID with ViewModel mapping
        public async Task<MasjidVisitViewModel?> GetByIdAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            return entity?.ToViewModel();
        }

        // Add a new Like using a ViewModel
        public async Task AddAsync(MasjidVisitCreateViewModel model)
        {
            var entity = model.ToEntity();
            await _baseRepo.AddAsync(entity);
            await _baseRepo.SaveChangesAsync();
        }

        // Delete a Like by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            if (entity == null)
                return false;

            _baseRepo.Delete(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }
    }
}
