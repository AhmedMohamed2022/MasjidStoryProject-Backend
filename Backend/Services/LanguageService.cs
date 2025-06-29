//using Models.Entities;
//using Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ViewModels;

//namespace Services
//{
//    public class LanguageService
//    {
//        private readonly IBaseRepository<Language> _baseRepo;

//        public LanguageService(IBaseRepository<Language> baseRepo)
//        {
//            _baseRepo = baseRepo;
//        }

//        public async Task<List<LanguageViewModel>> GetAllLanguagesAsync()
//        {
//            var langs = await _baseRepo.GetAllAsync();
//            return langs.Select(l => new LanguageViewModel
//            {
//                Id = l.Id,
//                Name = l.Name,
//                Code = l.Code
//            }).ToList();
//        }
//    }

//}
using Repositories.Interfaces;
using Models.Entities;

namespace Services
{
    public class LanguageService
    {
        private readonly IBaseRepository<Language> _languageRepo;

        public LanguageService(IBaseRepository<Language> languageRepo)
        {
            _languageRepo = languageRepo;
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            return await _languageRepo.FindAsync(l => true);
        }

        public async Task<Language?> GetLanguageByIdAsync(int id)
        {
            return await _languageRepo.GetByIdAsync(id);
        }

        public async Task<Language?> GetLanguageByCodeAsync(string code)
        {
            var languages = await _languageRepo.FindAsync(l => l.Code == code);
            return languages.FirstOrDefault();
        }
    }
}