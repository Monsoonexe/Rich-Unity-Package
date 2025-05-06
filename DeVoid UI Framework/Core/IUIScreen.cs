using System;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Interface that all UI Screens must implement in/directly.
	/// </summary>
	public interface IUIScreen
	{
		string ScreenID { get; set; }

		bool IsOpen { get; }

		void Show(IScreenProperties payload);

		void Hide(bool animate);

		event Action<IUIScreen> OnTransitionInFinishedCallback;

		event Action<IUIScreen> OnTransitionOutFinishedCallback;

		event Action<IUIScreen> OnScreenDestroyed;
	}
}
