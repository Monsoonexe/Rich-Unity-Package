using System;

namespace RichPackage.Editor.ValidationRules
{
    /// <seealso cref="AValidationRule"/>
    public class ValidationRuleAction
    {
        public readonly Action action;
        public readonly string name;

        public ValidationRuleAction(string name, Action action)
        {
            this.name = name;
            this.action = action;
        }

        public override string ToString() => name;
    }
}
