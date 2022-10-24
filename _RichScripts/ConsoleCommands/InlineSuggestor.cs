using QFSW.QC;
using System.Collections.Generic;

namespace ApexOfficer.ConsoleCommands
{
    /// <summary>
    /// Object that holds all the inlined suggestions provided to <see cref="SuggestionsAttribute"/>.
    /// </summary>
    public struct InlineSuggestionTag : IQcSuggestorTag
    {
        public readonly string[] Suggestions;

        public InlineSuggestionTag(string[] suggestions)
        {
            Suggestions = suggestions;
        }
    }

    public sealed class SuggestionsAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags;

        public SuggestionsAttribute(params string[] suggestions)
        {
            var tag = new InlineSuggestionTag(suggestions);
            _tags = new IQcSuggestorTag[] { tag };
        }

        public override IQcSuggestorTag[] GetSuggestorTags() => _tags;
    }

    /// <summary>
    /// 
    /// </summary>
    public class InlineSuggestor : IQcSuggestor
    {
        public IEnumerable<IQcSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            if (context.Tags != null)
            {
                foreach (var tag in context.Tags)
                {
                    if (tag is InlineSuggestionTag suggestorTag)
                    {
                        foreach (var s in suggestorTag.Suggestions)
                            yield return new RawSuggestion(s, true);
                    }
                }
            }
        }
    }
}
