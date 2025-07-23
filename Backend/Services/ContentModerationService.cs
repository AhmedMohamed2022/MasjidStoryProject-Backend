using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public class ContentModerationService
    {
        /// <summary>
        /// Determines if a story update requires re-approval based on content changes
        /// </summary>
        public ContentChangeAnalysis AnalyzeContentChanges(StoryEditViewModel editModel, StoryViewModel originalStory)
        {
            var analysis = new ContentChangeAnalysis
            {
                HasSignificantChanges = false,
                ChangeReason = new List<string>(),
                RequiresReapproval = false
            };

            // Compare all translations
            foreach (var editContent in editModel.Contents)
            {
                var origContent = originalStory.Contents?.FirstOrDefault(c => c.LanguageId == editContent.LanguageId);
                if (origContent == null)
                {
                    analysis.HasSignificantChanges = true;
                    analysis.ChangeReason.Add($"Added translation for language {editContent.LanguageId}");
                    continue;
                }
                if (!string.Equals(editContent.Title?.Trim(), origContent.Title?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    analysis.HasSignificantChanges = true;
                    analysis.ChangeReason.Add($"Title modified for language {editContent.LanguageId}");
                }
                var contentChange = AnalyzeContentChange(origContent.Content, editContent.Content);
                if (contentChange.IsSignificant)
                {
                    analysis.HasSignificantChanges = true;
                    analysis.ChangeReason.Add($"Content {contentChange.ChangeType} for language {editContent.LanguageId}");
                }
            }
            // Check for removed translations
            foreach (var origContent in originalStory.Contents)
            {
                if (!editModel.Contents.Any(c => c.LanguageId == origContent.LanguageId))
                {
                    analysis.HasSignificantChanges = true;
                    analysis.ChangeReason.Add($"Removed translation for language {origContent.LanguageId}");
                }
            }
            // Check image changes (unchanged)
            var originalImageCount = originalStory.MediaItems?.Count ?? 0;
            var newImageCount = (editModel.NewStoryImages?.Count ?? 0) + (editModel.KeepMediaIds?.Count ?? 0);
            if (Math.Abs(originalImageCount - newImageCount) > 0)
            {
                analysis.HasSignificantChanges = true;
                analysis.ChangeReason.Add("Images modified");
            }
            analysis.RequiresReapproval = analysis.HasSignificantChanges;
            return analysis;
        }

        private ContentChangeResult AnalyzeContentChange(string originalContent, string newContent)
        {
            if (string.IsNullOrEmpty(originalContent) && string.IsNullOrEmpty(newContent))
                return new ContentChangeResult { IsSignificant = false, ChangeType = "unchanged" };

            if (string.IsNullOrEmpty(originalContent) || string.IsNullOrEmpty(newContent))
                return new ContentChangeResult { IsSignificant = true, ChangeType = "completely changed" };

            // Normalize content for comparison
            var normalizedOriginal = NormalizeContent(originalContent);
            var normalizedNew = NormalizeContent(newContent);

            // Calculate similarity percentage
            var similarity = CalculateSimilarity(normalizedOriginal, normalizedNew);

            if (similarity >= 0.95) // 95% similar - minor changes
            {
                return new ContentChangeResult { IsSignificant = false, ChangeType = "minor edits" };
            }
            else if (similarity >= 0.8) // 80-95% similar - moderate changes
            {
                return new ContentChangeResult { IsSignificant = true, ChangeType = "moderately modified" };
            }
            else // Less than 80% similar - major changes
            {
                return new ContentChangeResult { IsSignificant = true, ChangeType = "significantly modified" };
            }
        }

        private string NormalizeContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // Remove extra whitespace, normalize line breaks, and convert to lowercase
            return content
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\t", " ")
                .ToLowerInvariant()
                .Trim();
        }

        private double CalculateSimilarity(string text1, string text2)
        {
            if (string.IsNullOrEmpty(text1) && string.IsNullOrEmpty(text2))
                return 1.0;

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
                return 0.0;

            // Simple Levenshtein distance-based similarity
            var distance = CalculateLevenshteinDistance(text1, text2);
            var maxLength = Math.Max(text1.Length, text2.Length);
            
            return maxLength == 0 ? 1.0 : 1.0 - ((double)distance / maxLength);
        }

        private int CalculateLevenshteinDistance(string s1, string s2)
        {
            int[,] d = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++)
                d[i, 0] = i;

            for (int j = 0; j <= s2.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[s1.Length, s2.Length];
        }
    }

    public class ContentChangeAnalysis
    {
        public bool HasSignificantChanges { get; set; }
        public bool RequiresReapproval { get; set; }
        public List<string> ChangeReason { get; set; } = new List<string>();
    }

    public class ContentChangeResult
    {
        public bool IsSignificant { get; set; }
        public string ChangeType { get; set; } = string.Empty;
    }
} 