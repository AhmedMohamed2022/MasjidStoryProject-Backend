using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class EventContent
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
