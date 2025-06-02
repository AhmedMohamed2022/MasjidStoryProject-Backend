using Models.Entities;
using ViewModels;
namespace Repositories.Interfaces
{
    /// <summary>
    /// Interface for custom Masjid repository operations (extends base CRUD).
    /// </summary>
    // Repositories/Interfaces/IMasjidRepository.cs
    public interface IMasjidRepository
    {
        Task<List<MasjidViewModel>> GetAllAsync();
        Task<List<MasjidViewModel>> GetFilteredAsync(string? search, int pageNumber, int pageSize);
        Task<MasjidViewModel?> GetByIdAsync(int id);
        Task<MasjidEditViewModel?> GetEditByIdAsync(int id);
        Task AddAsync(MasjidCreateViewModel model);
        Task<bool> UpdateAsync(MasjidEditViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<MasjidDetailsViewModel?> GetMasjidDetailsAsync(int id, string? languageCode = null);
    }

}
