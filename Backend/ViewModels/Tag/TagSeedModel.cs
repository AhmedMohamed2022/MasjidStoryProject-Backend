namespace ViewModels
{
    public class TagSeedModel
    {
        public string key { get; set; }
        public List<TagTranslationSeedModel> translations { get; set; }
    }
    public class TagTranslationSeedModel
    {
        public string languageCode { get; set; }
        public string name { get; set; }
    }
} 