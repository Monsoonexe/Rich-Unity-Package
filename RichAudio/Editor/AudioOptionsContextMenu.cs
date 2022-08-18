using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace RichPackage.Audio.Editor
{
	public sealed class AudioOptionsDrawer
		: OdinValueDrawer<AudioOptions>, 
		IDefinesGenericMenuItems
	{
		protected override void Initialize()
		{
			// We don't use this drawer to "draw" something so skip it when drawing.
			SkipWhenDrawing = true;
		}

		public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
		{
			genericMenu.AddItem(new GUIContent($"{nameof(ConfigureForDefaultSFX)}()"),
				on: false, () => ConfigureForDefaultSFX(property));

			genericMenu.AddItem(new GUIContent($"{nameof(ConfigureForDefaultBGM)}()"),
				on: false, () => ConfigureForDefaultBGM(property));
		}

		private void ConfigureForDefaultSFX(InspectorProperty property)
		{
			AudioOptions options = AudioOptions.DefaultSfx;
			property.ValueEntry.WeakValues[0] = options;
		}

		private void ConfigureForDefaultBGM(InspectorProperty property)
		{
			AudioOptions options = AudioOptions.DefaultBGM;
			property.ValueEntry.WeakValues[0] = options;
		}
	}
}
