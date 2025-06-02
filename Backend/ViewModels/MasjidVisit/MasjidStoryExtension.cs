using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class MasjidStoryExtension
    {
        public static MasjidVisitViewModel ToViewModel(this MasjidVisit entity)
        {
            return new MasjidVisitViewModel
            {
                Id = entity.Id,
                MasjidId = entity.MasjidId,
                UserId = entity.UserId,
                VisitDate = entity.VisitDate
            };
        }

        // Convert LikeCreateViewModel to Like entity
        public static MasjidVisit ToEntity(this MasjidVisitCreateViewModel model)
        {
            return new MasjidVisit
            {
                MasjidId=model.MasjidId,
                UserId = model.UserId,
                VisitDate = DateTime.UtcNow
            };
        }
        public static MasjidVisitCreateViewModel ToCreateViewModel(this MasjidVisit entity)
        {
            return new MasjidVisitCreateViewModel
            {
                UserId = entity.UserId,
                MasjidId = entity.MasjidId
            };
        }
    }
}
