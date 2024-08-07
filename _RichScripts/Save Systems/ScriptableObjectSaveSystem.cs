using UnityEngine;
using ScriptableObjectArchitecture;
using RichPackage.Events.Signals;
using System.Collections.Generic;

namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Saves ScriptableObjectArchitecture data.
	/// </summary>
	/// <seealso cref="SaveSystem"/>
	public class ScriptableObjectSaveSystem : RichMonoBehaviour
	{
		public List<BaseVariable> savedVariables;

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("Saves " +
				"ScriptableObjectArchitecture data.");
		}

		private void OnEnable()
		{
			//subscribe to save events
			GlobalSignals.Get<SaveStateToFileSignal>().AddListener(SaveState);
			GlobalSignals.Get<LoadStateFromFileSignal>().AddListener(LoadState);
		}

		private void OnDisable()
		{
			//ubsubscribe from save events
			GlobalSignals.Get<SaveStateToFileSignal>().RemoveListener(SaveState);
			GlobalSignals.Get<LoadStateFromFileSignal>().RemoveListener(LoadState);
		}

		private void SaveState(ISaveStore saveFile)
		{
			foreach(var datum in savedVariables)
			{
				SaveDatum(saveFile, datum);
			}
		}

		private void LoadState(ISaveStore saveFile)
		{
			foreach (var datum in savedVariables)
			{
				LoadDatum(saveFile, datum);
			}
		}

		private void LoadDatum(ISaveStore saveFile,
			BaseVariable variable)
		{
			string saveKey = variable.Name;
			if (variable is IntVariable intVar)
			{
				intVar.Value = saveFile.Load<int>(
					saveKey, intVar.Value);
			}
			else if (variable is FloatVariable floatVar)
			{
				floatVar.Value = saveFile.Load<float>(
					saveKey, floatVar.Value);
			}
			else if (variable is BoolVariable boolVar)
			{
				boolVar.Value = saveFile.Load<bool>(
					saveKey, boolVar.Value);
			}
			else if (variable is StringVariable stringVar)
			{
				stringVar.Value = saveFile.Load<string>(
					saveKey, stringVar.Value);
			}
			else
			{
				Debug.LogWarning("[ScriptableObjectSaveSystem]" +
					" Save Type not implemented. "
					+ "Rich is just being lazy.");
			}
		}

		private void SaveDatum(ISaveStore saveFile,
			BaseVariable variable)
		{
			string saveKey = variable.Name;
            switch (variable)
            {
                case IntVariable intVar:
                    saveFile.Save(saveKey, intVar.Value);
                    break;
                case FloatVariable floatVar:
                    saveFile.Save(saveKey, floatVar.Value);
                    break;
                case StringVariable stringVar:
                    saveFile.Save(saveKey, stringVar.Value);
                    break;
                case BoolVariable boolVar:
                    saveFile.Save(saveKey, boolVar.Value);
                    break;
                default:
                    Debug.LogError($"Cannot save {variable.GetType()}.", variable);
                    break;
            }
        }
	}
}
