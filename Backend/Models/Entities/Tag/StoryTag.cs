using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class StoryTag
    {
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
