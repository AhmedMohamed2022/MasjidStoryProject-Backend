using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    // StoryContent for multilingual story titles and content
    public class StoryContent
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
} 