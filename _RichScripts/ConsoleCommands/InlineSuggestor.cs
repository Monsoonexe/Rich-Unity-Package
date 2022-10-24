using QFSW.QC;
using System.Collections.Generic;
using System.Linq;

namespace ApexOfficer.ConsoleCommands
{
    /// <summary>
    /// Object that holds all the inlined suggestions provided to <see cref="SuggestionsAttribute"/>.
    /// </summary>
    public struct InlineSuggestionTag : IQcSuggestorTag
    {
        public readonly IEnumerable<string> Suggestions;

        public InlineSuggestionTag(IEnumerable<string> suggestions)
        {
            Suggestions = suggestions;
        }
    }

    public sealed class SuggestionsAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags;

        /// <param name="suggestions">String-convertible suggestions.</param>
        public SuggestionsAttribute(params object[] suggestions)
        {
            var tag = new InlineSuggestionTag(suggestions.Select(o => o.ToString()));
            _tags = new IQcSuggestorTag[] { tag };
        }

        public override IQcSuggestorTag[] GetSuggestorTags() => _tags;
    }

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
