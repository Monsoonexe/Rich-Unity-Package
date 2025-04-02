using UnityEngine;

namespace RichPackage.UI.Framework.Navigation
{
	/// <summary>
	/// Routes navigation requests to the correct screen with the proper properties.
	/// </summary>
	public abstract class ANavigationProvider : MonoBehaviour
	{
		public abstract void NavigateTo(string screenID);
	}
}
