using System.Collections.Generic;

namespace RichPackage.Editor.ValidationRules
{
    /// <summary>
    /// Classes that implement this interface will be injected into the <see cref="ValidationRulesWindow"/>
    /// and can be used to expose issues with assets and configurations.
    /// </summary>
    /// <seealso cref="ValidationRulesWindow"/>
    /// <seealso cref="AValidationRule"/>
    public interface IValidationRule
    {
        /// <summary>
        /// Key for saving in editor prefs.
        /// </summary>
        string SaveName { get; }

        /// <summary>
        /// Ignore the issue if it is present.
        /// </summary>
        bool Ignore { get; set; }

        /// <summary>
        /// Issues associated with this rule.
        /// </summary>
        List<ValidationRuleIssue> Issues { get; }

        /// <summary>
        /// Checks whether the issue is present.
        /// </summary>
        void Validate();
    }
}
