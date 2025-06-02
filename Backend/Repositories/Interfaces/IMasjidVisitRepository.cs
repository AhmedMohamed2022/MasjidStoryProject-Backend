using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repositories.Interfaces
{
    public interface IMasjidVisitRepository
    {
        Task<List<MasjidVisitViewModel>> GetAllAsync();
        Task<MasjidVisitViewModel?> GetByIdAsync(int id);
        Task AddAsync(MasjidVisitCreateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
