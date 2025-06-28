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
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string MasjidName { get; set; }
        public int? MasjidId { get; set; }
        public string CreatedByName { get; set; }
        public bool IsUserRegistered { get; set; } // optional for frontend toggle
    }


}
