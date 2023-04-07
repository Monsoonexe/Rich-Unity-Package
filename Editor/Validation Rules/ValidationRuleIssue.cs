using UnityEditor;

namespace RichPackage.Editor.ValidationRules
{
    public class ValidationRuleIssue
    {
        public readonly string message;
        public readonly MessageType messageType;
        public readonly ValidationRuleAction[] actions;

        public ValidationRuleIssue(string message, MessageType messageType, params ValidationRuleAction[] actions)
        {
            this.message = message;
            this.messageType = messageType;
            this.actions = actions;
        }

        public override string ToString()
        {
	        bool isIssue = messageType == MessageType.Warning || messageType == MessageType.Error;
            string messageTypeString = isIssue ? messageType + ": " : "";
            return $"{messageTypeString}{message}";
        }
    }
}
