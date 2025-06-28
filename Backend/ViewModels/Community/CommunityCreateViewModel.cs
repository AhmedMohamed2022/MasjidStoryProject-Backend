using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CommunityCreateViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int MasjidId { get; set; }
        public int LanguageId { get; set; }
    }

}
