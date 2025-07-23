using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // Tags to categorize stories
    public class Tag
    {
        public int Id { get; set; }
        // Remove flat Name property
        // public string Name { get; set; }
        public virtual ICollection<TagContent> Contents { get; set; }
        public virtual ICollection<StoryTag> StoryTags { get; set; }
    }
}
