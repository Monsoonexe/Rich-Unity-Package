using Sirenix.OdinInspector;

namespace RichPackage.UI
{
	/// <summary>
	/// A button that raises an object event when pressed.
	/// </summary>
	public class ObjectButton : RichUIButton<object>
	{
		//exists

#if UNITY_EDITOR

		[ShowInInspector, ReadOnly]
		public string Payload { get => PayloadData?.ToString() ?? "nada"; }

#endif
	}
}
