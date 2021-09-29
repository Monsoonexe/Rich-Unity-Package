using UnityEngine;
using Sirenix.OdinInspector;
using Signals;
using RichPackage.SaveSystem.Signals;

namespace RichPackage.SaveSystem
{
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
		[Tooltip("Must be unique to all other saveables!")]
		protected string saveID;

		/// <summary>
		/// A persistent, unique string identifier.
		/// </summary>
		public string SaveID { get => saveID; }

		protected virtual void Reset()
		{
			saveID = gameObject.name;
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
			//=> mySaveData = saveFile.Load(saveID, mySaveData);

		public void DeleteState(ES3File saveFile)
			=> saveFile.DeleteKey(saveID);

#if UNITY_EDITOR
		[Button("Is ID Unique?")]
		public void Editor_PrintIDIsUnique()
		{
			if (IsSaveIDUnique(this))
			{
				Debug.Log("I am unique!");
			}
			else
			{
				Debug.Log("Name collision! uniqueID is already taken.");
			}
		}
#endif

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
