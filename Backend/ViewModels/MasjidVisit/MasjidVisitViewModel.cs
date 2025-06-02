using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class MasjidVisitViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MasjidId { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
    }
}
