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

		private void Reset()
		{
			SetDevDescription("Saves " +
				"ScriptableObjectArchitecture data.");
		}

		private void OnEnable()
		{
			//subscribe to save events
			GlobalSignals.Get<SaveStateToFile>().AddListener(SaveState);
			GlobalSignals.Get<LoadStateFromFile>().AddListener(LoadState);
		}

		private void OnDisable()
		{
			//ubsubscribe from save events
			GlobalSignals.Get<SaveStateToFile>().RemoveListener(SaveState);
			GlobalSignals.Get<LoadStateFromFile>().RemoveListener(LoadState);
		}

		private void SaveState(ES3File saveFile)
		{
			foreach(var datum in savedVariables)
			{
				SaveDatum(saveFile, datum);
			}
		}

		private void LoadState(ES3File saveFile)
		{
			foreach (var datum in savedVariables)
			{
				LoadDatum(saveFile, datum);
			}
		}

		private void LoadDatum(ES3File saveFile,
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

		private void SaveDatum(ES3File saveFile,
			BaseVariable variable)
		{
			string saveKey = variable.Name;
			if (variable is IntVariable intVar)
			{
				saveFile.Save<int>(saveKey, intVar.Value);
			}
			else if (variable is FloatVariable floatVar)
			{
				saveFile.Save<float>(saveKey, floatVar.Value);
			}
			else if (variable is StringVariable stringVar)
			{
				saveFile.Save<string>(saveKey, stringVar.Value);
			}
			else if (variable is BoolVariable boolVar)
			{
				saveFile.Save<bool>(saveKey, boolVar.Value);
			}
			else
			{
				Debug.LogWarning("[ScriptableObjectSaveSystem]" +
					" Save Type not implemented. "
					+ "Rich is just being lazy.");
			}
		}
	}

}
