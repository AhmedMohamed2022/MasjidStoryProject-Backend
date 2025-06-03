using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Models.Entities;
using System.Linq.Expressions;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations
{
    // Repositories/Implementations/MasjidRepository.cs
    public class MasjidRepository : IMasjidRepository
    {
        private readonly IBaseRepository<Masjid> _baseRepo;
        private readonly ApplicationDbContext _context;
        public MasjidRepository(IBaseRepository<Masjid> baseRepo, ApplicationDbContext context)
        {
            _baseRepo = baseRepo;
            _context = context;
        }

        public async Task<List<MasjidViewModel>> GetAllAsync()
        {
            var list = await _baseRepo.GetAllAsync(m => m.Country, m => m.City);
            return list.Select(m => m.ToViewModel()).ToList();
        }
        public async Task<List<Masjid>> GetAllAsync(params Expression<Func<Masjid, object>>[] includes)
        {
            return await _baseRepo.GetAllAsync(includes);
        }


        public async Task<List<MasjidViewModel>> GetFilteredAsync(string? search, int pageNumber, int pageSize)
        {
            Expression<Func<Masjid, bool>> filter = m =>
                string.IsNullOrEmpty(search) || m.ShortName.Contains(search);

            var masjids = await _baseRepo.GetListWithIncludePagedAsync(
                filter,
                pageNumber,
                pageSize,
                m => m.Country,
                m => m.City
            );

            return masjids.Select(m => m.ToViewModel()).ToList();
        }

        public async Task<MasjidViewModel?> GetByIdAsync(int id)
        {
            var entity = await _baseRepo.GetFirstOrDefaultAsync(
                m => m.Id == id,
                m => m.Country, m => m.City);
            return entity?.ToViewModel();
        }

        public async Task<MasjidEditViewModel?> GetEditByIdAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            return entity == null ? null : new MasjidEditViewModel
            {
                Id = entity.Id,
                ShortName = entity.ShortName,
                Address = entity.Address,
                ArchStyle = entity.ArchStyle,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                CountryId = entity.CountryId,
                CityId = entity.CityId,
                YearOfEstablishment = entity.YearOfEstablishment
            };
        }

        public async Task AddAsync(MasjidCreateViewModel model)
        {
            var entity = model.ToEntity();
            await _baseRepo.AddAsync(entity);
            await _baseRepo.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(MasjidEditViewModel model)
        {
            var entity = await _baseRepo.GetByIdAsync(model.Id);
            if (entity == null) return false;
            model.UpdateEntity(entity);
            _baseRepo.Update(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _baseRepo.GetByIdAsync(id);
            if (entity == null) return false;
            _baseRepo.Delete(entity);
            await _baseRepo.SaveChangesAsync();
            return true;
        }
  
        public async Task<MasjidDetailsViewModel?> GetMasjidDetailsAsync(int id, string? languageCode = null)
        {
            var masjid = await _context.Masjids
                .Where(m => m.Id == id)
                .Include(m => m.Country)
                .Include(m => m.City)
                .Include(m => m.Contents)
                .Include(m => m.MediaItems)
                .Include(m => m.Visits)
                .Include(m => m.Events)
                .Include(m => m.Stories)
                    .ThenInclude(s => s.ApplicationUser)
                .Include(m => m.Stories)
                    .ThenInclude(s => s.Likes)
                .Include(m => m.Stories)
                    .ThenInclude(s => s.Comments)
                .FirstOrDefaultAsync();

            return masjid?.ToDetailsViewModel(languageCode);
        }


    }


}
