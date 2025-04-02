using System;
using System.Collections.Generic;

namespace RichPackage.UI.Framework.Navigation
{
	/// <summary>
	/// A <see cref="ANavigationProvider"/> that caches a mapping of screenIds to methods that open the window.
	/// </summary>
	public abstract class ACachedNavigationProvider : ANavigationProvider
	{
		protected readonly Dictionary<string, Action> windowMapping
			= new Dictionary<string, Action>();

		public override void NavigateTo(string screenID)
		{
			OpenWindow(screenID);
		}

		protected void MapWindow(string screenID, Action opener)
		{
			// TODO - error handling
			windowMapping.Add(screenID, opener);
		}

		protected void OpenWindow(string screenID)
		{
			// TODO - error handling
			windowMapping[screenID]();
		}
	}
}
