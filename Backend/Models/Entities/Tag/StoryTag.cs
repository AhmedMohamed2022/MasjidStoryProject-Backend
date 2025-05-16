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
        public Story Story { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
