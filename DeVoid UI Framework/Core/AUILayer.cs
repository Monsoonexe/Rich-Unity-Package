using RichPackage.GuardClauses;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Base class for UI Layers. Layers implement custom logic for Screens when opening, closing, etc.
	/// A Layer is responsible for containing and controlling a specific type of Screen.
	/// Communicates with the Screens themselves.
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class AUILayer<TScreen> : MonoBehaviour
		where TScreen : class, IUIScreen
	{
		private const string FunctionBoxGroup = "Functions";

		/// <summary>
		/// Collection of Screens.
		/// </summary>
		protected readonly Dictionary<string, TScreen> registeredScreens = new Dictionary<string, TScreen>();
		protected UIFrame frame;

		[ShowInInspector, ReadOnly]
		public int ScreenCount => registeredScreens.Count;

		protected void OnDestroy()
		{
			registeredScreens.Clear();
		}

		/// <summary>
		/// Shows given Screen.
		/// </summary>
		public abstract void ShowScreen(TScreen screen);

		/// <summary>
		/// Shows given Screen and passes in given Properties.
		/// </summary>
		/// <typeparam name="TProps">Type of data payload</typeparam>
		/// <param name="properties">the data payload</param>
		public abstract void ShowScreen(TScreen screen, IScreenProperties properties);

		/// <summary>
		/// Hides given Screen.
		/// </summary>
		public abstract void HideScreen(TScreen screen, bool animate = true);

		/// <summary>
		/// Inits this Layer.
		/// </summary>
		/// <remarks>Inheritors should call base.</remarks>
		public void Initialize(UIFrame frame)
		{
			this.frame = frame;
		}

		/// <summary>
		/// Reparents given Transform to this Layer.
		/// </summary>
		public virtual void ReparentScreen(IUIScreen screen,
			Transform screenTransform)
		{
			// it's okay to have parameters that don't get used this go around.
			screenTransform.SetParent(transform, worldPositionStays: false);
		}

		#region Screen Registration

		public bool IsScreenRegistered(string screenID)
		{
			return registeredScreens.ContainsKey(screenID);
		}

		/// <summary>
		/// Returns an array of all the ScreenIDs that are registered to this layer.
		/// </summary>
		public IEnumerable<string> GetRegisteredScreenIDs()
			=> registeredScreens.Values.Select((s) => s.ScreenID);

		/// <summary>
		/// Register a Screen this this Layer.
		/// </summary>
		public void RegisterScreen(string screenID, TScreen screen)
		{
			// validate
			GuardAgainst.IsNullOrEmpty(screenID, nameof(screenID));
			Assert.IsFalse(IsScreenRegistered(screenID), $"screenID <{screenID}> " +
				"is already registered to this Layer.");

			// operate
			ProcessScreenRegister(screenID, screen);
		}

		public void UnregisterScreen(string screenID, TScreen screen)
		{
			Assert.IsTrue(IsScreenRegistered(screenID), $"screenID: <{screenID}> " +
				"is not registered to this Layer!");

			ProcessScreenUnregister(screenID, screen);
		}

		protected virtual void ProcessScreenRegister(string screenID, TScreen screen)
		{
			screen.ScreenID = screenID;
			registeredScreens.Add(screenID, screen);
			screen.OnScreenDestroyed += OnScreenDestroyed;
		}

		protected virtual void ProcessScreenUnregister(string screenID, TScreen screen)
		{
			screen.OnScreenDestroyed -= OnScreenDestroyed;
			registeredScreens.Remove(screenID);
		}

		#endregion Screen Registration

		#region Show/Hide Screens

		[Button, DisableInEditorMode, FoldoutGroup(FunctionBoxGroup)]
		public void ShowScreenByID(string screenID) => ShowScreenByID(screenID, null);

		public void ShowScreenByID(string screenID, IScreenProperties properties)
		{
			if (!registeredScreens.TryGetValue(screenID, out TScreen screen))
				throw GetScreenNotRegisteredException(screenID);

			ShowScreen(screen, properties);
		}

		[Button, DisableInEditorMode, FoldoutGroup(FunctionBoxGroup)]
		public void HideScreenByID(string screenID, bool animate = true)
		{
			if (!registeredScreens.TryGetValue(screenID, out TScreen screen))
				throw GetScreenNotRegisteredException(screenID);

			HideScreen(screen, animate);
		}

		[Button, DisableInEditorMode, FoldoutGroup(FunctionBoxGroup)]
		public virtual void HideAll(bool animateWhenHiding = true)
		{
			foreach (KeyValuePair<string, TScreen> screenEntry in registeredScreens)
			{
				HideScreen(screenEntry.Value, animateWhenHiding);
			}
		}

		#endregion Show/Hide Screens

		private void OnScreenDestroyed(IUIScreen screen)
		{
			if (!string.IsNullOrEmpty(screen.ScreenID)
				&& registeredScreens.ContainsKey(screen.ScreenID))
			{
				UnregisterScreen(screen.ScreenID, (TScreen)screen);
			}
		}

		private static System.Exception GetScreenNotRegisteredException(string screenID)
		{
			return new System.Exception($"[{nameof(AUILayer<TScreen>)}] screenID: " +
				$"<{screenID}> is not registered to this Layer.");
		}

		[Button, DisableInEditorMode, FoldoutGroup(FunctionBoxGroup)]
		public void PrintRegisteredScreenIDs()
		{
			if (registeredScreens.Count == 0)
			{
				Debug.Log("No screens are registered to " + this.name);
			}
			else
			{
				Debug.Log($"{name} has {ScreenCount.ToStringCached()} registered screens:");
				foreach (KeyValuePair<string, TScreen> screenEntry in registeredScreens)
				{
					Debug.Log(screenEntry.Value.ScreenID);
				}
			}
		}
	}
}