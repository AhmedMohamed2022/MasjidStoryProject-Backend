using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CommunityCreateViewModel
    {
        public int MasjidId { get; set; }
        public List<CommunityContentCreateViewModel> Contents { get; set; }
    }

}
