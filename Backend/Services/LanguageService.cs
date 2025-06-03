using Models.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class LanguageService
    {
        private readonly IBaseRepository<Language> _baseRepo;

        public LanguageService(IBaseRepository<Language> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public async Task<List<LanguageViewModel>> GetAllLanguagesAsync()
        {
            var langs = await _baseRepo.GetAllAsync();
            return langs.Select(l => new LanguageViewModel
            {
                Id = l.Id,
                Name = l.Name,
                Code = l.Code
            }).ToList();
        }
    }

}
