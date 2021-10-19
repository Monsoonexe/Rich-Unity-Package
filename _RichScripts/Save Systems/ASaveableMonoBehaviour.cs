using System.Diagnostics;
using UnityEngine;
using Sirenix.OdinInspector;
using Signals;
using RichPackage.SaveSystem.Signals;

namespace RichPackage.SaveSystem
{
	using Debug = UnityEngine.Debug;
	/* private struct SaveData 
	 * {
	 *		data goes here
	 * }
	 * public override void SaveState(ES3File saveFile);
			=> saveFile.Save(saveID, mySaveData); 
	 * public override void LoadState(ES3File saveFile);
			=> mySaveData = saveFile.Load(saveID, mySaveData);
	 * 
	 */

	/// <summary>
	/// Base class with everything you need to save some persistent data.
	/// </summary>
	public abstract class ASaveableMonoBehaviour : RichMonoBehaviour
	{
		[SerializeField]
		[CustomContextMenu("Set to Name", "SetDefaultSaveID")]
		[CustomContextMenu("Set to GUID", "SetSaveIDToGUID")]
		[Tooltip("Must be unique to all other saveables!")]
		protected string saveID;

		/// <summary>
		/// A persistent, unique string identifier.
		/// </summary>
		public string SaveID { get => saveID; }

		protected virtual void Reset()
		{
			SetDefaultSaveID();
		}

		protected virtual void OnEnable()
		{
			//subscribe to save events
			GlobalSignals.Get<SaveStateToFile>().AddListener(SaveState);
			GlobalSignals.Get<LoadStateFromFile>().AddListener(LoadState);
		}

		protected virtual void OnDisable()
		{
			//ubsubscribe from save events
			GlobalSignals.Get<SaveStateToFile>().RemoveListener(SaveState);
			GlobalSignals.Get<LoadStateFromFile>().RemoveListener(LoadState);
		}

		public virtual void SetDefaultSaveID()
		{
			saveID = gameObject.name;
			Editor_PrintIDIsUnique();
		}

		public void SetSaveIDToGUID()
		{
			saveID = System.Guid.NewGuid().ToString();
			Editor_PrintIDIsUnique();
		}

		/// <summary>
		/// Must be overriden with proper derived type of <cref="SaveData"/>
		/// </summary>
		/// <param name="saveFile"></param>
		public abstract void SaveState(ES3File saveFile);
			//=> saveFile.Save(saveID, mySaveData); //recommended code

		/// <summary>
		/// Must be overriden with proper derived type of <cref="SaveData"/>
		/// </summary>
		/// <param name="saveFile"></param>
		public abstract void LoadState(ES3File saveFile);
			//=> mySaveData = saveFile.Load(saveID, mySaveData); //recommended code

		public void DeleteState(ES3File saveFile)
			=> saveFile.DeleteKey(saveID);

		[Button("Is ID Unique?")]
		[Conditional(ConstStrings.UNITY_EDITOR)]
		public void Editor_PrintIDIsUnique()
		{
			if (IsSaveIDUnique(this))
			{
				Debug.Log("ID is unique (in this scene)!");
			}
			else
			{
				Debug.Log("Name collision! uniqueID is already taken.");
			}
		}

		public static bool IsSaveIDUnique(ASaveableMonoBehaviour query)
		{
			var allSaveables = FindObjectsOfType<ASaveableMonoBehaviour>();
			bool isUnique = true; //return value
			foreach (var saveable in allSaveables)
			{
				if (saveable.saveID == query.saveID
					&& saveable != query) //check is not self
				{
					isUnique = false;
					break;
				}
			}
			return isUnique;
		}
	}
}
