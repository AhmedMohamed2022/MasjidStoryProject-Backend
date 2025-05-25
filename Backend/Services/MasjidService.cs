using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using Repositories.Implementations;
using Repositories.Interfaces;
using ViewModels;
namespace Services
{
    public class MasjidService
    {
        private readonly IMasjidRepository _repository;

        public MasjidService(IMasjidRepository repository)
        {
            _repository = repository;
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
    }

}
