/* reference: https://www.qfsw.co.uk/docs/QC/articles/extending/suggestors.html
 * Classes to help suggest ScreenIDs in Quantum Console when working with UIFrame commands.
 */

using QFSW.QC;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Tag. Not sure why this is needed.
/// </summary>
public struct ScreenIDTag : IQcSuggestorTag
{
    // exists
}

/// <summary>
/// Use like `void SomeFunc([<see cref="ScreenIDAttribute"/>] param) { }`
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ScreenIDAttribute : SuggestorTagAttribute
{
    private readonly IQcSuggestorTag[] _tags = { new ScreenIDTag() };

    public override IQcSuggestorTag[] GetSuggestorTags()
    {
        return _tags;
    }
}

public class ScreenIDSuggestor : BasicCachedQcSuggestor<string>
{
    protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
    {
        return context.HasTag<ScreenIDTag>();
    }

    protected override IQcSuggestion ItemToSuggestion(string screenID)
    {
        return new RawSuggestion(screenID, singleLiteral: true);
    }

    protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
    {
        return QuantumRegistry.GetRegistryContents<UIFrame>()
            .FirstOrDefault()
            ?.GetRegisteredScreenIDs()
            ?? Array.Empty<string>();
    }
}
