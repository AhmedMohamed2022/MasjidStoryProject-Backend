using System;

namespace ViewModels
{
    public class CommunityContentViewModel
    {
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class CommunityContentCreateViewModel
    {
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
} 