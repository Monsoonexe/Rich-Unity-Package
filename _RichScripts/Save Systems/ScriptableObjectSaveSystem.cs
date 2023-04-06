using UnityEngine;
using ScriptableObjectArchitecture;
using RichPackage.Events.Signals;
using RichPackage.SaveSystem.Signals;

namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Saves ScriptableObjectArchitecture data.
	/// </summary>
	/// <seealso cref="SaveSystem"/>
	public class ScriptableObjectSaveSystem : RichMonoBehaviour
	{
		[SerializeField]
		private BaseVariable[] savedVariables;

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("Saves ScriptableObjectArchitecture data.");
		}

		private void OnEnable()
		{
			// subscribe to save events
			GlobalSignals.Get<SaveStateToFileSignal>().AddListener(SaveState);
			GlobalSignals.Get<LoadStateFromFileSignal>().AddListener(LoadState);
		}

		private void OnDisable()
		{
			// unsubscribe from save events
			GlobalSignals.Get<SaveStateToFileSignal>().RemoveListener(SaveState);
			GlobalSignals.Get<LoadStateFromFileSignal>().RemoveListener(LoadState);
		}

		private void SaveState(ISaveSystem saveFile)
		{
			foreach(var datum in savedVariables)
			{
				SaveDatum(saveFile, datum);
			}
		}

		private void LoadState(ISaveSystem saveFile)
		{
			foreach (var datum in savedVariables)
			{
				LoadDatum(saveFile, datum);
			}
		}

		private void LoadDatum(ISaveSystem saveFile,
			BaseVariable variable)
		{
			string saveKey = variable.Name;
            switch (variable)
            {
                case IntVariable intVar:
                    intVar.Value = saveFile.Load(saveKey, intVar.Value);
                    break;
                case FloatVariable floatVar:
                    floatVar.Value = saveFile.Load(saveKey, floatVar.Value);
                    break;
                case BoolVariable boolVar:
                    boolVar.Value = saveFile.Load(saveKey, boolVar.Value);
                    break;
                case StringVariable stringVar:
                    stringVar.Value = saveFile.Load(saveKey, stringVar.Value);
                    break;
                default:
                    Debug.LogError($"Cannot save {variable.GetType()}.", variable);
                    break;
            }
        }

		private void SaveDatum(ISaveSystem saveFile,
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
