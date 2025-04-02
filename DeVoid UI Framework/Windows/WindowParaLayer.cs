using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Layer components will reparent screens to this.
	/// </summary>
	/// <seealso cref="WindowUILayer"/>
	public class WindowParaLayer : MonoBehaviour
	{
		[SerializeField, Required]
		private GameObject darkenBackgroundObject = null;

		private readonly List<GameObject> containedScreens = new List<GameObject>();

		public void AddScreen(Transform screenRect)
		{
			screenRect.SetParent(transform, false);
			containedScreens.Add(screenRect.gameObject);
		}

		public void RefreshDarken()
		{
			for (int i = containedScreens.Count - 1; i >= 0; --i)
			{
				GameObject screen = containedScreens[i];

				if (screen.activeSelf)
				{
					DarkenBackground();
					return;
				}
			}

			darkenBackgroundObject.SetActive(false);
		}

		public void DarkenBackground()
		{
			darkenBackgroundObject.SetActive(true);
			darkenBackgroundObject.transform.SetAsLastSibling();
		}
	}
}
