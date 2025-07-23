using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string LocalizedTitle { get; set; }
        public string LocalizedDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string MasjidName { get; set; }
        public int? MasjidId { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedById { get; set; }
        public bool IsUserRegistered { get; set; } // optional for frontend toggle
        public List<EventContentViewModel> Contents { get; set; } = new();
    }


}
