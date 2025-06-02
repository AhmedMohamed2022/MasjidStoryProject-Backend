using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class MasjidVisitCreateViewModel
    {
        [Required]
        public int MasjidId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
