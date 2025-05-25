using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    // ViewModel used when editing an existing comment
    public class CommentEditViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }

}
