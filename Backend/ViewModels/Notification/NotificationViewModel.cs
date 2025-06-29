using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // "Like", "Comment", "Approval", "General"
        public string ContentType { get; set; } // "Story", "Event", "Community"
        public int? ContentId { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
        public string SenderName { get; set; }
    }

    public class NotificationCreateViewModel
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
        public int? ContentId { get; set; }
        public string SenderName { get; set; }
    }

    public class NotificationUpdateViewModel
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
    }
} 